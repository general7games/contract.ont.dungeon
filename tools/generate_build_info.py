import sys
import time
from git import Repo

repo = Repo('.', search_parent_directories=True)

print('Generate build info start.')
path = sys.argv[1]
template_path = path + '.template'
print(template_path, '->', path)

with open(template_path, 'r') as f:
    content = f.read()
    content = content.replace('#BUILD_DATE#', str(int(time.time())))
    content = content.replace('#GIT_COMMIT#', repo.git.rev_parse(repo.head, short=True))
with open(path, 'w') as f:
    f.write(content)

print('Generate build info end.')