if [ ! -f assets/infore/hifigan/g_01140000 ]; then
  echo "Downloading models..."
  mkdir -p assets/infore/{nat,hifigan}
  curl -o "assets/infore/nat/duration_latest_ckpt.pickle" -L "https://huggingface.co/ntt123/viettts_infore_16k/resolve/main/duration_latest_ckpt.pickle"
  curl -o "assets/infore/nat/acoustic_latest_ckpt.pickle" -L "https://huggingface.co/ntt123/viettts_infore_16k/resolve/main/acoustic_latest_ckpt.pickle"
  curl -o "assets/infore/hifigan/g_01140000" -L "https://huggingface.co/ntt123/viettts_infore_16k/resolve/main/g_01140000"
  python -m vietTTS.hifigan.convert_torch_model_to_haiku --config-file=assets/hifigan/config.json --checkpoint-file=assets/infore/hifigan/g_01140000
fi

# echo "Generate audio clip"
# text=`cat "assets/transcript.txt"`
# python -m vietTTS.synthesizer --text "$text" --output assets/infore/00002.wav --lexicon-file assets/infore/lexicon.txt --silence-duration 0.2
# python -m vietTTS.synthesizer --text "hôm qua em tới trường" --output assets/infore/clip.wav --lexicon-file assets/infore/lexicon.txt --silence-duration 0.2