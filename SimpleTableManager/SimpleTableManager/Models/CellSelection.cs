using SimpleTableManager.Models.CommandExecuters;

namespace SimpleTableManager.Models;

public class CellSelection
{
	private bool _isPrimarySelected = false;

	private int _secondarySelectionCount = 0;

	private int _tertiarySelectionCount = 0;

	public bool IsPrimarySelected => _isPrimarySelected;

	public bool IsNotPrimarySelected => !_isPrimarySelected;

	public bool IsSecondarySelected => _secondarySelectionCount > 0;

	public bool IsNotSecondarySelected => _secondarySelectionCount == 0;

	public bool IsTertiarySelected => _tertiarySelectionCount > 0;

	public bool IsNotTertiarySelected => _tertiarySelectionCount == 0;

	public CellSelectionLevel GetHighestSelectionLevel()
	{
		if (IsPrimarySelected)
		{
			return CellSelectionLevel.Primary;
		}
		else if (IsSecondarySelected)
		{
			return CellSelectionLevel.Secondary;
		}
		else if (IsTertiarySelected)
		{
			return CellSelectionLevel.Tertiary;
		}
		else
		{
			return CellSelectionLevel.None;
		}
	}

	public void SelectPrimary()
	{
		_isPrimarySelected = true;
	}

	public void DeselectPrimary()
	{
		_isPrimarySelected = false;
	}

	public void SelectSecondary()
	{
		_secondarySelectionCount++;
	}

	public void DeselectSecondary()
	{
		if (_secondarySelectionCount > 0)
		{
			_secondarySelectionCount--;
		}
	}

	public void SelectTertiary()
	{
		_tertiarySelectionCount++;
	}

	public void DeselectTertiary()
	{
		if (_tertiarySelectionCount > 0)
		{
			_tertiarySelectionCount--;
		}
	}
}