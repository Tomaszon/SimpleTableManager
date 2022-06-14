using System.Collections.Generic;

namespace SimpleTableManager.Services;

public class RenderFunction
{
	public static RenderFunction Default => new RenderFunction(RendererType.Default);

	public RendererType RendererType { get; set; }

	public RenderFunction(RendererType rendererType)
	{
		RendererType = rendererType;
	}

	public List<object> Render(List<object> content)
	{
		if (RendererType == RendererType.Default)
		{
			return content;
		}
		else
		{
			throw new System.NotImplementedException("Nyaa :3");
		}
	}
}

public enum RendererType
{
	Default,
	Function
}
