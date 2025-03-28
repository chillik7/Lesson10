using System;

public sealed class AudioPlayer
{
    private static AudioPlayer _instance;

    private static readonly object _lock = new object();

    private string _currentTrack;

    private bool _isPlaying;

    private AudioPlayer()
    {
        _currentTrack = string.Empty;
        _isPlaying = false;
    }

    public static AudioPlayer Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new AudioPlayer();
                    }
                }
            }
            return _instance;
        }
    }

    public void Play(string track)
    {
        if (_isPlaying)
        {
            Stop();
        }

        _currentTrack = track;
        _isPlaying = true;
        Console.WriteLine($"Воспроизводится: {_currentTrack}");
    }

    public void Stop()
    {
        if (_isPlaying)
        {
            _isPlaying = false;
            Console.WriteLine($"Воспроизведение остановлено: {_currentTrack}");
            _currentTrack = string.Empty;
        }
        else
        {
            Console.WriteLine("Нет активного воспроизведения");
        }
    }

    public string GetStatus()
    {
        return _isPlaying
            ? $"Сейчас играет: {_currentTrack}"
            : "Воспроизведение не активно";
    }
}

class Program
{
    static void Main(string[] args)
    {
        AudioPlayer player1 = AudioPlayer.Instance;
        AudioPlayer player2 = AudioPlayer.Instance;

        Console.WriteLine($"player1 и player2 - один и тот же объект? {player1 == player2}");

        player1.Play("Queen - Bohemian Rhapsody");
        player2.Play("The Beatles - Yesterday");

        Console.WriteLine(player1.GetStatus());

        player1.Stop();
        player2.Stop();

        Console.WriteLine(player2.GetStatus());
    }
}