using NetCoreAudio;

namespace SimpleTableManager.Services;

/// <summary>
/// Plays sound files
/// </summary>
public class SmartPlayer(Note[] notes)
{
	private readonly SemaphoreSlim _semaphoreSlim = new(1);

	private readonly Note[] _notes = notes;

	/// <summary>
	/// Plays a sound configured amount of times
	/// </summary>
	public async Task PlayAsync()
	{
		await Task.Run(() => Shared.IndexArray(_notes.Length).ForEach(async i => await EnqueueAsync(_notes[i])));
	}

	/// <summary>
	/// Enqueues a file based play task
	/// </summary>
	private async Task EnqueueAsync(Note note)
	{
		try
		{
			var player = new Player();

			await player.SetVolume(Settings.Current.Volume);

			player.PlaybackFinished += Finished;

			var fileName = $"Audio/{(int)note}.250.mp3";

			await _semaphoreSlim.WaitAsync();

			await player.Play(fileName);
		}
		catch { }
	}

	/// <summary>
	/// Releases the lock used for play task queuing
	/// </summary>
	private void Finished(object? sender, EventArgs e)
	{
		_semaphoreSlim.Release();
	}
}