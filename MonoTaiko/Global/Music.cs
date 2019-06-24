using NAudio.Vorbis;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoTaiko.Global
{
    class Music
    {
        static readonly object audioLock = new object();
        public static WaveOutEvent musicOut = new WaveOutEvent();

        public static bool isPlaying;

        public static void Play(VorbisWaveReader audio)
        {
#pragma warning disable
            audio_init(audio, 0.5f, 0f);
            isPlaying = true;
        }

        public static void Play(VorbisWaveReader audio, float volume)
        {
#pragma warning disable
            audio_init(audio, volume, 0f);
            isPlaying = true;
        }

        public static void Play(VorbisWaveReader audio, float volume, float startPos)
        {
#pragma warning disable
            audio_init(audio, volume, startPos);
            isPlaying = true;
        }

        public static void Play(LoopStream audio)
        {
#pragma warning disable
            audio_init(audio, 0.5f, 0f);
            isPlaying = true;
        }

        public static void Stop(bool dispose)
        {
            musicOut.Stop();
            isPlaying = false;

            if (dispose) musicOut.Dispose();
        }

        private static async Task audio_init(VorbisWaveReader audio, float volume, float startPos)
        {
            await Task.Run(() =>
            {
                lock (audioLock)
                {
                    if (musicOut.PlaybackState == PlaybackState.Stopped)
                    {
                        musicOut.Init(audio);
                        audio.CurrentTime = TimeSpan.FromSeconds(startPos);
                        musicOut.Volume = volume;
                        musicOut.Play();
                        return;
                    }
                    else return;
                }
            });
        }

        private static async Task audio_init(LoopStream audio, float volume, float startPos)
        {
            await Task.Run(() =>
            {
                lock (audioLock)
                {
                    if (musicOut.PlaybackState == PlaybackState.Stopped)
                    {
                        musicOut.Init(audio);
                        audio.CurrentTime = TimeSpan.FromSeconds(startPos);
                        musicOut.Volume = volume;
                        musicOut.Play();
                        return;
                    }
                    else return;
                }
            });
        }
    }
}
