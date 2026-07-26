import os

def translate_file(filepath, replacements):
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()

    for k, v in replacements.items():
        content = content.replace(k, v)

    with open(filepath, 'w', encoding='utf-8') as f:
        f.write(content)
    print(f"Translated {filepath}")

cps_replacements = {
    '"help           — daftar perintah"': '"help           — command list"',
    '"status         — cek status sistem"': '"status         — check system status"',
    '"open           — buka kasus (mode kasus)"': '"open           — open case (case mode)"',
    '"info           — lihat data subjek"': '"info           — view subject data"',
    '"check          — periksa foto subjek"': '"check          — inspect subject photo"',
    '"right / left    — putar foto subjek (saat check)"': '"right / left    — rotate subject photo (when checking)"',
    '"back           — kembali ke menu utama kasus"': '"back           — return to main case menu"',
    '"ERR: ID tidak ditemukan — "': '"ERR: ID not found — "',
    '" sudah di-lock (gagal maksimal)."': '" is locked (max failures)."',
    '"TGL LAHIR    : "': '"DOB          : "',
    '"> Periksa data & foto. Ketik approved atau denied."': '"> Inspect data & photo. Type approved or denied."',
    '"ERR: tidak ada subject aktif. Gunakan fetch [ID] dulu."': '"ERR: no active subject. Use fetch [ID] first."',
    '"[BENAR] Verifikasi akurat."': '"[CORRECT] Verification accurate."',
    '"ERR: perintah tidak dikenal — \\""': '"ERR: unknown command — \\""',
    '"> Ketik help untuk daftar perintah."': '"> Type help for command list."'
}

gm_replacements = {
    '"Semua kasus selesai. Menunggu perintah..."': '"All cases completed. Awaiting commands..."'
}

cm_replacements = {
    '"Terima kasih. Subjek telah dikembalikan. Shift berlanjut."': '"Thank you. Subject returned. Shift continues."'
}

translate_file("Assets/Scripts/Terminal/CommandProcessorService.cs", cps_replacements)
translate_file("Assets/Scripts/GameManagerComputer/GameManager.cs", gm_replacements)
translate_file("Assets/Scripts/Terminal/CaseManager.cs", cm_replacements)
