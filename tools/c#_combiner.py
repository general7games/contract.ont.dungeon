import sys

print('C# combine start.')
if len(sys.argv) < 3:
    raise Exception('Please input multi C# files.')

using_content = ''
main_content = ''
for i in range(1, len(sys.argv)):
    path = sys.argv[i]
    print('Open', path)
    with open(path, 'r') as f:
        while True:
            line = f.readline()
            if not line:
                break
            if line.startswith('using '):
                if line not in using_content:
                    using_content = using_content + line
            else:
                main_content = main_content + line

print('Write to _combined.cs')
with open('_combined.cs', 'w') as f:
    f.write(using_content)
    f.write(main_content)

print('C# combine end.')