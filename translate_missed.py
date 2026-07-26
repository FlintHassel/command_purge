import os

def translate_file(filepath, replacements):
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()

    for k, v in replacements.items():
        content = content.replace(k, v)

    with open(filepath, 'w', encoding='utf-8') as f:
        f.write(content)
    print(f"Translated {filepath}")

opening_replacements = {
    '"Identitas terverifikasi."': '"Identity verified."',
    '"Selamat bekerja, Verifier "': '"Welcome to work, Verifier "',
    '"iya"': '"yes"',
    '"tidak"': '"no"',
    '"Coba dengarkan baik-baik..."': '"Listen carefully..."',
    '"Ketik \'iya\' atau \'tidak\'."': '"Type \'yes\' or \'no\'."',
    '"Baik, kita mulai."': '"Alright, let\'s begin."',
    '"Ketik \'open\' untuk membuka folder."': '"Type \'open\' to open the folder."',
    '"Konfirmasi diterima: "': '"Confirmation received: "',
    '"Sistem sedang memproses..."': '"System is processing..."',
    '"[ MEMBACA DATA... ]"': '"[ READING DATA... ]"',
    '"SUKSES."': '"SUCCESS."',
    '"GAGAL."': '"FAILED."',
    '"Selesai. Tunggu instruksi selanjutnya."': '"Done. Await further instructions."',
    '"Kembali ke seleksi awal..."': '"Returning to initial selection..."',
    '"Subject dipilih: "': '"Subject selected: "',
    '"Ketik \'confirm\' untuk mencetak dokumen."': '"Type \'confirm\' to print document."',
    '"Ketik \'typetest\' untuk lanjut mencetak."': '"Type \'typetest\' to continue printing."',
    '"Ketik \'typetest\' untuk lanjut."': '"Type \'typetest\' to continue."'
}

terminal_replacements = {
    '"INVESTIGASI SUBJEK"': '"SUBJECT INVESTIGATION"',
    '"JAWABAN SUBJEK"': '"SUBJECT ANSWERS"',
    '"(ketik perintah di terminal)"': '"(type a command in the terminal)"',
    '"(Ketik \'print\' di terminal untuk mencetak dokumen keputusan)"': '"(Type \'print\' in the terminal to print the decision document)"',
    '"Subjek akan dikembalikan.\\nKetik \'confirm\' untuk memastikan keputusan."': '"Subject will be returned.\\nType \'confirm\' to confirm the decision."',
    '"SISTEM: Penolakan tidak diizinkan oleh otoritas pusat."': '"SYSTEM: Rejection is not permitted by central authority."',
    '"izinkan masuk"': '"allow entry"',
    '"tolak & kembalikan"': '"reject & return"',
    '"Kesempatan investigasi habis. Ambil keputusan:"': '"Investigation attempts exhausted. Make a decision:"',
    '"Ketik"': '"Type"',
    '"Kembali"': '"Back"',
    '"Tutup"': '"Close"',
    '"Buka"': '"Open"',
    '"Cetak"': '"Print"',
    '"Konfirmasi"': '"Confirm"',
    '"Sistem"': '"System"',
    '"Anda"': '"You"',
    '"Tugas"': '"Task"',
    '"Lahir"': '"Born"',
    '"Perintah"': '"Command"',
    '"Gagal"': '"Failed"',
    '"Berhasil"': '"Success"'
}

translate_file("Assets/Scripts/Terminal/OpeningSequence.cs", opening_replacements)
translate_file("Assets/Scripts/Terminal/TerminalController.cs", terminal_replacements)

# ComputerController.cs replacements
computer_replacements = {
    '"Press [ESC] to Quit"': '"Press [ESC] to Quit"',
    '"Press [E] to Use Computer"': '"Press [E] to Use Computer"'
}
translate_file("Assets/Scripts/Interact Object/Komputer/ComputerController.cs", computer_replacements)
