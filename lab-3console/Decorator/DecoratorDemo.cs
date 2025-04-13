using System;

public interface IHero
{
    string GetName();
    string GetDescription();
}

public class Warrior : IHero
{
    public string GetName() => "Warrior";
    public string GetDescription() => "A mighty warrior";
}

public class Mage : IHero
{
    public string GetName() => "Mage";
    public string GetDescription() => "A wise mage";
}

public class Palladin : IHero
{
    public string GetName() => "Palladin";
    public string GetDescription() => "A holy palladin";
}

public abstract class HeroDecorator : IHero
{
    protected IHero hero;

    public HeroDecorator(IHero hero)
    {
        this.hero = hero;
    }

    public virtual string GetName()
    {
        return hero.GetName();
    }

    public virtual string GetDescription()
    {
        return hero.GetDescription();
    }
}

public class ArmorDecorator : HeroDecorator
{
    public ArmorDecorator(IHero hero) : base(hero) { }

    public override string GetDescription()
    {
        return hero.GetDescription() + ", wearing sturdy armor";
    }
}

public class WeaponDecorator : HeroDecorator
{
    public WeaponDecorator(IHero hero) : base(hero) { }

    public override string GetDescription()
    {
        return hero.GetDescription() + ", equipped with a powerful weapon";
    }
}

public class ArtifactDecorator : HeroDecorator
{
    private string artifactName;
    public ArtifactDecorator(IHero hero, string artifactName) : base(hero)
    {
        this.artifactName = artifactName;
    }

    public override string GetDescription()
    {
        return hero.GetDescription() + $", holding artifact '{artifactName}'";
    }
}

public class DecoratorDemo
{
    public static void RunDemo()
    {
        Console.WriteLine("=== Decorator Demo ===");

        IHero warrior = new Warrior();
        IHero mage = new Mage();
        IHero palladin = new Palladin();

        IHero warriorWithArmor = new ArmorDecorator(warrior);
        IHero mageWithArtifact = new ArtifactDecorator(mage, "Ancient Book");
        IHero palladinFullGear = new WeaponDecorator(new ArmorDecorator(new ArtifactDecorator(palladin, "Holy Grail")));

        Console.WriteLine($"{warriorWithArmor.GetName()} -> {warriorWithArmor.GetDescription()}");
        Console.WriteLine($"{mageWithArtifact.GetName()} -> {mageWithArtifact.GetDescription()}");
        Console.WriteLine($"{palladinFullGear.GetName()} -> {palladinFullGear.GetDescription()}");
    }
}