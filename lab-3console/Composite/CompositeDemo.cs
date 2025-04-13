using System;
using System.Collections.Generic;
using System.Text;

public abstract class LightNode
{
    public abstract string GetOuterHTML();
    public abstract string GetInnerHTML();
}

public class LightTextNode : LightNode
{
    private string text;
    public LightTextNode(string text)
    {
        this.text = text;
    }

    public override string GetOuterHTML()
    {
        return text;
    }

    public override string GetInnerHTML()
    {
        return text;
    }
}

public enum DisplayType
{
    Block,
    Inline
}

public enum ClosingType
{
    Normal,
    SelfClosing
}

public class LightElementNode : LightNode
{
    public string TagName { get; set; }
    public DisplayType Display { get; set; }
    public ClosingType CloseType { get; set; }
    public List<string> CssClasses { get; set; }
    private List<LightNode> children;

    public LightElementNode(string tagName, DisplayType display, ClosingType closeType)
    {
        TagName = tagName;
        Display = display;
        CloseType = closeType;
        CssClasses = new List<string>();
        children = new List<LightNode>();
    }

    public void AddChild(LightNode node)
    {
        children.Add(node);
    }

    public override string GetOuterHTML()
    {
        if (CloseType == ClosingType.SelfClosing)
        {
            return $"<{TagName}{GetClassString()}/>";
        }
        else
        {
            return $"<{TagName}{GetClassString()}>{GetInnerHTML()}</{TagName}>";
        }
    }

    public override string GetInnerHTML()
    {
        if (CloseType == ClosingType.SelfClosing)
            return string.Empty;

        StringBuilder sb = new StringBuilder();
        foreach (var child in children)
        {
            sb.Append(child.GetOuterHTML());
        }
        return sb.ToString();
    }

    private string GetClassString()
    {
        if (CssClasses.Count == 0) return "";
        return $" class=\"{string.Join(" ", CssClasses)}\"";
    }
}

public class CompositeDemo
{
    public static void RunDemo()
    {
        Console.WriteLine("=== Composite Demo (LightHTML) ===");

        var div = new LightElementNode("div", DisplayType.Block, ClosingType.Normal);
        div.CssClasses.Add("container");

        var p = new LightElementNode("p", DisplayType.Block, ClosingType.Normal);
        p.AddChild(new LightTextNode("Hello "));

        var span = new LightElementNode("span", DisplayType.Inline, ClosingType.Normal);
        span.AddChild(new LightTextNode("World!"));

        p.AddChild(span);
        div.AddChild(p);

        string resultHtml = div.GetOuterHTML();
        Console.WriteLine(resultHtml);
    }
}