import sys
import json
import requests

print('Online compiler start.')
csharp_path = sys.argv[1]
output_path = sys.argv[2]

print('Read combined C# file', csharp_path)
with open(csharp_path, "r") as f:
    csharp_content = f.read()

j_data = {
    'code': csharp_content,
    'type': 'CSharp'
}
r = requests.post('https://smartxcompiler.ont.io/api/v1.0/csharp/compile', json=j_data, verify=False)
if r.status_code != 200:
    raise Exception('Response code ' + r.status_code)

j_text = r.json()
if j_text['errcode'] != 0:
    raise Exception('Compile error, ' + r.text)

avm_path = output_path + '.avm'
print('Write', avm_path)
with open(avm_path, 'w') as f:
    f.write(j_text['avm'][2:-1])

abi_path = output_path + '.abi.json'
print('Write', abi_path)
with open(abi_path, 'w') as f:
    abi = j_text['abi'][2:-1].replace('\\n', '')
    f.write(json.dumps(json.loads(abi), indent=2))

print('Online compiler end.')