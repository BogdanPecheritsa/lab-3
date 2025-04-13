using System;
using System.Collections.Generic;
using System.Text;

public class LightElementFlyweight
{
    public string TagName { get; private set; }
    public DisplayType Display { get; private set; }
    public ClosingType CloseType { get; private set; }

    public LightElementFlyweight(string tagName, DisplayType display, ClosingType closeType)
    {
        TagName = tagName;
        Display = display;
        CloseType = closeType;
    }
}

public class LightElementFlyweightFactory
{
    private Dictionary<string, LightElementFlyweight> flyweights = new Dictionary<string, LightElementFlyweight>();

    public LightElementFlyweight GetFlyweight(string key, Func<LightElementFlyweight> createFunc)
    {
        if (!flyweights.ContainsKey(key))
        {
            flyweights[key] = createFunc();
        }
        return flyweights[key];
    }
}

public class FlyweightElementNode : LightNode
{
    private LightElementFlyweight flyweight;
    private List<LightNode> children = new List<LightNode>();
    private List<string> cssClasses = new List<string>();

    public FlyweightElementNode(LightElementFlyweight flyweight)
    {
        this.flyweight = flyweight;
    }

    public void AddChild(LightNode child)
    {
        children.Add(child);
    }

    public void AddCssClass(string cls)
    {
        cssClasses.Add(cls);
    }

    public override string GetOuterHTML()
    {
        if (flyweight.CloseType == ClosingType.SelfClosing)
        {
            return $"<{flyweight.TagName}{GetClassString()}/>";
        }
        else
        {
            return $"<{flyweight.TagName}{GetClassString()}>{GetInnerHTML()}</{flyweight.TagName}>";
        }
    }

    public override string GetInnerHTML()
    {
        if (flyweight.CloseType == ClosingType.SelfClosing) return string.Empty;

        StringBuilder sb = new StringBuilder();
        foreach (var c in children)
        {
            sb.Append(c.GetOuterHTML());
        }
        return sb.ToString();
    }

    private string GetClassString()
    {
        if (cssClasses.Count == 0) return "";
        return $" class=\"{string.Join(" ", cssClasses)}\"";
    }
}

public class FlyweightDemo
{
    public static void RunDemo()
    {
        Console.WriteLine("=== Flyweight Demo ===");

        string[] lines = {
            "The Great Book",
            "Chapter One",
            " This line starts with a space",
            "Here is a normal line with more than 20 chars",
            "Short line"
        };

        Console.WriteLine("---- Without Flyweight (Simple) ----");
        foreach (var line in lines)
        {
            LightNode node = ConvertLineToHTML(line);
            Console.WriteLine(node.GetOuterHTML());
        }

        Console.WriteLine("\n---- With Flyweight ----");
        var factory = new LightElementFlyweightFactory();
        foreach (var line in lines)
        {
            LightNode node = ConvertLineToHTMLFlyweight(line, factory);
            Console.WriteLine(node.GetOuterHTML());
        }
    }

    private static int lineIndex = 0;

    private static LightNode ConvertLineToHTML(string line)
    {
        LightElementNode elem;
        if (lineIndex == 0)
        {
            elem = new LightElementNode("h1", DisplayType.Block, ClosingType.Normal);
        }
        else if (line.StartsWith(" "))
        {
            elem = new LightElementNode("blockquote", DisplayType.Block, ClosingType.Normal);
        }
        else if (line.Length < 20)
        {
            elem = new LightElementNode("h2", DisplayType.Block, ClosingType.Normal);
        }
        else
        {
            elem = new LightElementNode("p", DisplayType.Block, ClosingType.Normal);
        }

        lineIndex++;
        elem.AddChild(new LightTextNode(line));
        return elem;
    }

    private static int lineIndexFlyweight = 0;
    private static LightNode ConvertLineToHTMLFlyweight(string line, LightElementFlyweightFactory factory)
    {
        LightElementFlyweight flyweight;
        if (lineIndexFlyweight == 0)
        {
            flyweight = factory.GetFlyweight("h1-block-normal", () => new LightElementFlyweight("h1", DisplayType.Block, ClosingType.Normal));
        }
        else if (line.StartsWith(" "))
        {
            flyweight = factory.GetFlyweight("blockquote-block-normal", () => new LightElementFlyweight("blockquote", DisplayType.Block, ClosingType.Normal));
        }
        else if (line.Length < 20)
        {
            flyweight = factory.GetFlyweight("h2-block-normal", () => new LightElementFlyweight("h2", DisplayType.Block, ClosingType.Normal));
        }
        else
        {
            flyweight = factory.GetFlyweight("p-block-normal", () => new LightElementFlyweight("p", DisplayType.Block, ClosingType.Normal));
        }

        lineIndexFlyweight++;

        var node = new FlyweightElementNode(flyweight);
        node.AddChild(new LightTextNode(line));
        return node;
    }
}