using System;

public interface IRenderer
{
    void RenderCircle();
    void RenderSquare();
    void RenderTriangle();
}

public class VectorRenderer : IRenderer
{
    public void RenderCircle()
    {
        Console.WriteLine("Drawing Circle as vectors.");
    }

    public void RenderSquare()
    {
        Console.WriteLine("Drawing Square as vectors.");
    }

    public void RenderTriangle()
    {
        Console.WriteLine("Drawing Triangle as vectors.");
    }
}

public class RasterRenderer : IRenderer
{
    public void RenderCircle()
    {
        Console.WriteLine("Drawing Circle as pixels.");
    }

    public void RenderSquare()
    {
        Console.WriteLine("Drawing Square as pixels.");
    }

    public void RenderTriangle()
    {
        Console.WriteLine("Drawing Triangle as pixels.");
    }
}

public abstract class Shape
{
    protected IRenderer renderer;
    public Shape(IRenderer renderer)
    {
        this.renderer = renderer;
    }

    public abstract void Draw();
}

public class Circle : Shape
{
    public Circle(IRenderer renderer) : base(renderer) { }
    public override void Draw()
    {
        renderer.RenderCircle();
    }
}

public class Square : Shape
{
    public Square(IRenderer renderer) : base(renderer) { }
    public override void Draw()
    {
        renderer.RenderSquare();
    }
}

public class Triangle : Shape
{
    public Triangle(IRenderer renderer) : base(renderer) { }
    public override void Draw()
    {
        renderer.RenderTriangle();
    }
}

public class BridgeDemo
{
    public static void RunDemo()
    {
        Console.WriteLine("=== Bridge Demo ===");

        IRenderer vectorRenderer = new VectorRenderer();
        IRenderer rasterRenderer = new RasterRenderer();

        Shape circleVector = new Circle(vectorRenderer);
        Shape squareRaster = new Square(rasterRenderer);
        Shape triangleVector = new Triangle(vectorRenderer);

        circleVector.Draw();
        squareRaster.Draw();
        triangleVector.Draw();
    }
}