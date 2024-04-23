from flask import Flask, request, send_file
from io import BytesIO
import soundfile as sf
from vietTTS.synthesizer import nat_normalize_text, text2mel, mel2wave

app = Flask(__name__)

@app.route('/', methods=['POST'])
def synthesize():
    if 'text_file' not in request.files:
        return {'error': 'No text file provided'}, 400
    text_file = request.files['text_file']
    output = "../assets/infore/" + text_file.filename.split('.')[0] + ".wav"
    
    lexicon_file = '../assets/infore/lexicon.txt'
    silence_duration = 0.2
    text_input = text_file.read().decode('utf-8')
    # return {'text_input': text_input,'output_path': output}
    
    if text_input:
        text = nat_normalize_text(text_input)
        print("Normalizing text input...")
        mel = text2mel(text, lexicon_file, silence_duration)
        print("Converting to audio...")
        wave = mel2wave(mel)
        print("Writing output to file ", output)
        sf.write(str(output), wave, 16000)
        return send_file(output, download_name=output, mimetype='audio/wav')
    else:
        return {'error': 'No text file provided'}, 400

if __name__ == '__main__':
    app.run()