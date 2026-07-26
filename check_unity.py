import os
import re

search_pattern = re.compile(r'(?i)(Masukan|Masukkan|Lanjutkan|Pengaturan|Jeda|Keluar|Mulai|Kembali)')

found_any = False
for r, d, fs in os.walk('Assets'):
    for f in fs:
        if f.endswith('.unity') or f.endswith('.prefab') or f.endswith('.asset'):
            filepath = os.path.join(r, f)
            try:
                with open(filepath, 'r', encoding='utf-8', errors='ignore') as file:
                    for i, l in enumerate(file):
                        if search_pattern.search(l):
                            print(f'{filepath}:{i+1} - {l.strip()}')
                            found_any = True
            except Exception as e:
                pass

if not found_any:
    print("No Indonesian strings found!")
