import os

def translate_file(filepath):
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()

    replacements = {
        '"ERR: Data subject tidak ditemukan."': '"ERR: Subject data not found."',
        '"Folder berkedip. Ketik \'open\' untuk membuka kasus."': '"Folder is blinking. Type \'open\' to open the case."',
        '">>> MEMBUKA KASUS #"': '">>> OPENING CASE #"',
        '"SUBJEK: "': '"SUBJECT: "',
        '"Ketik \'back\' untuk kembali."': '"Type \'back\' to return."',
        '"Ketik \'right\' / \'left\' buat putar, \'back\' buat kembali."': '"Type \'right\' / \'left\' to rotate, \'back\' to return."',
        '"SISTEM: Subjek ini telah diverifikasi oleh otoritas pusat. Penolakan tidak diizinkan."': '"SYSTEM: This subject has been verified by the central authority. Rejection is not allowed."',
        '"Perintah tidak dikenal. Ketik ask / info / check / traits / print."': '"Unknown command. Type ask / info / check / traits / print."',
        '"Kesempatan habis. Ambil keputusan: approved / denied."': '"Attempts exhausted. Make a decision: approved / denied."',
        '"[Sisa Baterai / Kesempatan: "': '"[Battery / Attempts Remaining: "',
        '"Ketik kata kunci pertanyaan yang mau ditanyakan, atau \'back\' untuk batal."': '"Type the keyword of the question you want to ask, or \'back\' to cancel."',
        '"Kata kunci tidak dikenal. Cek daftar pertanyaan di panel."': '"Unknown keyword. Check the question list in the panel."',
        '"Kamu: "': '"You: "',
        '"Ketik ulang pertanyaan di atas untuk memanggil jawaban."': '"Retype the question above to prompt an answer."',
        '"[!!!] Sinyal terganggu..."': '"[!!!] Signal disrupted..."',
        '"Sinyal kembali normal."': '"Signal restored to normal."',
        '"Keputusan telah dicatat ke sistem pusat."': '"Decision has been logged to the central system."',
        '"Ketik \'print\' untuk mencetak dokumen."': '"Type \'print\' to print the document."',
        '"Harap ketik \'confirm\' untuk konfirmasi pengembalian subjek."': '"Please type \'confirm\' to confirm subject return."',
        '"Mencetak..."': '"Printing..."',
        '"Silakan ambil dokumen di printer luar."': '"Please take the document from the external printer."',
        '"Ketik \'esc\' untuk kembali ke Desktop."': '"Type \'esc\' to return to the Desktop."'
    }

    for k, v in replacements.items():
        content = content.replace(k, v)

    with open(filepath, 'w', encoding='utf-8') as f:
        f.write(content)
    print(f"Translated {filepath}")

translate_file("Assets/Scripts/Terminal/CaseManager.cs")
