using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions.Markup.Renderers;

internal class CompositeRenderable : Renderable
{
    private readonly IEnumerable<IRenderable> _renderables;

    public CompositeRenderable(IEnumerable<IRenderable> renderables)
    {
        _renderables = renderables;
    }

    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth) =>
        _renderables.SelectMany(x => x.Render(options, maxWidth));
}