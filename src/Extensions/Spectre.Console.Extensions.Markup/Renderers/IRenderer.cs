using Markdig.Syntax;
using Spectre.Console.Rendering;

namespace Spectre.Console.Extensions.Markup.Renderers;
internal interface IRenderer<TBlock> where TBlock : Block
{
    IRenderable Render(TBlock block, BlockRenderer blockRenderer);
}
