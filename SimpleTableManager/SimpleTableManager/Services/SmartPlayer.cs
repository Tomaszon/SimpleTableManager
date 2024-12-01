using System.Diagnostics;
using NetCoreAudio;

namespace SimpleTableManager.Services;

public class SmartPlayer
{
	private readonly SemaphoreSlim _semaphoreSlim = new(1);

	private readonly Note[] _notes;

	private byte _volume = 25;

	public SmartPlayer(Note[] notes)
	{
		_notes = notes;
	}

	public async Task Play()
	{
		await Task.Run(() => Shared.IndexArray(_notes.Length).ForEach(async i => await Enqueue(_notes[i])));
	}

	public void SetVolume(byte volume)
	{
		_volume = volume;
	}

	private async Task Enqueue(Note note)
	{
		try
		{
			var player = new Player();

			await player.SetVolume(_volume);

			player.PlaybackFinished += Finished;

			var fileName = $"Audio/{(int)note}.250.mp3";

			await _semaphoreSlim.WaitAsync();

			await player.Play(fileName);
		}
		catch { }
	}

	private void Finished(object? sender, EventArgs e)
	{
		_semaphoreSlim.Release();
	}
}