import os
import re

found_any = False
for r, d, fs in os.walk('Assets/Scripts'):
    for f in fs:
        if f.endswith('.cs'):
            filepath = os.path.join(r, f)
            with open(filepath, 'r', encoding='utf-8') as file:
                lines = file.readlines()
                for i, l in enumerate(lines):
                    if re.search(r'\"[^\"]*(Ketik|Iya|Tidak|Kembali|Tutup|Buka|Konfirmasi|Sistem|Subjek|Anda|Tugas|Lahir|Perintah|Gagal|Berhasil|benar)[^\"]*\"', l, re.IGNORECASE):
                        print(f'{filepath}:{i+1} - {l.strip()}')
                        found_any = True

if not found_any:
    print("No Indonesian strings found!")
