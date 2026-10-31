"""Run against a fresh local verification instance, never a user's live store."""
import html
import http.cookiejar
import re
import sys
from html.parser import HTMLParser
from urllib.error import HTTPError
from urllib.parse import urlencode
from urllib.request import build_opener, HTTPCookieProcessor

base = sys.argv[1] if len(sys.argv) > 1 else 'http://localhost:5214'
opener = build_opener(HTTPCookieProcessor(http.cookiejar.CookieJar()))
prefix = '/DELMUNDOMARCKEVIN_241639_'

def get(path):
    return opener.open(base + path).read().decode()

def post(path, data, token_page=None, page=None):
    page = page or get(token_page or path)
    token = re.search(r'name="__RequestVerificationToken"[^>]*value="([^"]+)"', page)
    assert token, f'Missing antiforgery token on {path}'
    payload = dict(data, __RequestVerificationToken=html.unescape(token[1]))
    return opener.open(base + path, urlencode(payload).encode()).read().decode()

class Links(HTMLParser):
    def __init__(self, page):
        super().__init__()
        self.links = []
        self.feed(page)
    def handle_starttag(self, tag, attrs):
        attrs = dict(attrs)
        if tag == 'a' and 'program-row' in attrs.get('class', '').split():
            self.links.append(attrs['href'])

home = get('/')
assert '<title>FUNDAMENTAL OF PROGRAMMING</title>' in home
study = get(prefix + 'InputOutput/StudyTime')
assert all(subject in study for subject in ['Programming Fundamentals', 'Mathematics', 'English'])
assert 'Form reset.' in get(prefix + 'InputOutput/StudyTime?notice=reset')
links = Links(home).links
assert len(links) == len(set(links)) == 25
for path in links:
    page = get(path)
    assert '<main ' in page and 'FUNDAMENTAL OF PROGRAMMING' in page, path

for action, data, expected in [
    ('LargestSmallest', {'Input': '-2,4,9'}, 'Largest: 9'),
    ('SumAverage', {'Input': '1,2,3'}, 'Sum: 6'),
    ('ReverseArray', {'Input': '1,2,3,4'}, '4, 3, 2, 1'),
    ('BubbleSort', {'Input': '3,-2,3,0'}, '-2, 0, 3, 3'),
    ('MatrixAddition', {'Input': '1,2;3,4', 'SecondInput': '5,6;7,8'}, '6  8\n10  12'),
]:
    page = post(prefix + 'Arrays/' + action, data)
    assert expected in html.unescape(page), action
    assert 'Calculation complete.' in page, action
    assert '<summary>How it works</summary>' in page and '<summary>C# code</summary>' in page, action
    assert 'btn-copy-code' in page, action
assert 'same size' in post(prefix + 'Arrays/MatrixAddition', {'Input': '1,2', 'SecondInput': '1;2'})
assert 'alert-danger' in post(prefix + 'Arrays/SumAverage', {'Input': 'NaN'})
assert 'alert-danger' in post(prefix + 'Arrays/SumAverage', {'Input': ''})
for action in ['NumberGuessing', 'PinVerification']:
    page = get(prefix + 'Repetition/' + action)
    assert '<summary>How it works</summary>' in page and '<summary>C# code</summary>' in page
    assert 'do' in page and '} while' in page
assert 'Every guess must be' in post(prefix + 'Repetition/NumberGuessing', {'guesses': '1,x'})
assert 'Attempt' in post(prefix + 'Repetition/NumberGuessing', {'guesses': '25,50,75'})
pin_path = prefix + 'Repetition/PinVerification'
pin_page = get(pin_path)
demo_pin = re.search(r'Demo PIN: <strong[^>]*>([0-9]{4})</strong>', pin_page)[1]
wrong_pin = '0000' if demo_pin != '0000' else '1111'
locked = post(pin_path, {'pin': ','.join([wrong_pin] * 3)}, page=pin_page)
assert 'Three incorrect attempts' in locked
assert 'demo is locked' in post(pin_path, {'pin': demo_pin}, page=locked)
reset = post(pin_path, {'action': 'reset'}, page=locked)
verified = post(pin_path, {'pin': wrong_pin + ',' + demo_pin + ',' + wrong_pin}, page=reset)
assert 'Access granted. PIN verified.' in verified
assert 'Attempt 3:' not in verified.split('<summary>How it works</summary>')[0]
try:
    opener.open(base + prefix + 'Arrays/SumAverage', b'Input=1')
    raise AssertionError('Missing antiforgery token was accepted')
except HTTPError as error:
    assert error.code == 400

for controller in ['Library', 'Wishlist', 'LostFound', 'Attendance', 'GameCollection']:
    get(prefix + controller + '/Create')
    get(prefix + controller + '/Edit/1')
    invalid = post(prefix + controller + '/Create', {})
    assert 'alert-danger' in invalid, controller

name = 'Fundamentals HTTP verification book'
created = post(prefix + 'Library/Create', {'title': name, 'author': 'Test Author', 'genre': 'Test', 'year': '2026'})
assert name in created
assert 'class="app-toast" id="app-toast" role="status"' in created
row = next(row for row in re.findall(r'<tr.*?</tr>', created, re.S) if name in row)
edit = re.search(r'href="([^"]+/Edit/\d+)"', row)[1]
record_id = edit.rsplit('/', 1)[1]
updated = post(edit, {'id': record_id, 'title': name + ' edited', 'author': 'Test Author', 'genre': 'Test', 'year': '2026'})
assert name + ' edited' in updated
deleted = post(prefix + 'Library/Delete/' + record_id, {'id': record_id}, prefix + 'Library')
assert name not in deleted
print('HTTP checks passed: 25 catalog pages, array and loop forms, invalid input, antiforgery, all CRUD form pages, and library create/edit/delete.')
