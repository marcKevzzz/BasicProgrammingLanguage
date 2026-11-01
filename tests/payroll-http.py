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

home = get('/')
payroll = prefix + 'Conditional/Payroll'
payslip = post(payroll, {'salary': '30000.00'})
for value in ['1,500.00', '750.00', '200.00', '27,550.00', '1,007.55', '26,542.45']:
    assert value in payslip, value
assert 'salary-error' in post(payroll, {'salary': 'invalid'})
assert 'Payroll Calculator' in get(prefix + 'Conditional/InternetPlan')
assert home.index('Password Strength Checker') < home.index('Payroll Calculator')
for action in ['Maze', 'TheaterSeating', 'TicTacToe']:
    lesson = get(prefix + 'Arrays/' + action)
    assert '<summary>C# code</summary>' in lesson and 'array-code-highlight' in lesson
print('Payroll HTTP validation, sample payslip, legacy redirect, navigation, and 2D lessons passed.')
