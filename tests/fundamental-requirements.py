from pathlib import Path
import re

root = Path(__file__).resolve().parents[1]
prefix = 'DELMUNDOMARCKEVIN_241639_'
app = root / (prefix + 'FUNDAMENTAL_OF_PROGRAMMING')
assert app.is_dir(), 'Application folder must use the requested naming convention'
catalog = (app / 'Services/ProgramCatalog.cs').read_text(encoding='utf-8-sig')
for name in ['Basic Input-Output', 'Nested If…Else', 'Loops', 'Arrays', 'CRUD']:
    assert name in catalog, f'Missing category: {name}'
for path in (app / 'Controllers').glob('*.cs'):
    assert path.name.startswith(prefix), path
for path in (app / 'Views').rglob('*.cshtml'):
    if path.name not in ('_ViewStart.cshtml', '_ViewImports.cshtml'):
        assert path.name.startswith(prefix), path
assert len(list((app / 'Models').glob(prefix + '*.cs'))) > 0
assert len(re.findall(r'\["categoryId"\] = "arrays"', catalog)) == 5
print('Naming and PDF category requirements passed.')
