using System;
public class Rogalik
{
    public static void Main(string[] args)
    {
        Game game = new Game();
        game.Start();
    }
}
public class Game
{
    private Player Player;
    public Game()
    {
        Console.WriteLine("Добро пожаловать, воин!\nНазови себя:");
        string playerName = Console.ReadLine();
        Aid aidKit = new Aid("Средняя аптечка", 20);
        Weapon weapon = new Weapon("Меч Фламберг", 15, 30);
        Player = new Player(playerName, 100, aidKit, weapon);
        Console.WriteLine($"Ваше имя {Player.Name}!");
        Console.WriteLine($"Вам был ниспослан {Player.Weapon.Name} ({Player.Weapon.Damage}), а также {Player.AidKit.Name} ({Player.AidKit.HealAmount}hp).");
        Console.WriteLine($"У вас {Player.MaxHealth}hp.\n");
    }
    public void Start()
    {
        while (Player.CurrentHealth > 0)
        {
            Enemy enemy = GenerateRandomEnemy();
            Console.WriteLine($"Nata встречает врага {enemy.Name} ({enemy.CurrentHealth}hp), у врага на поясе сияет оружие {enemy.Weapon.Name} ({enemy.Weapon.Damage})");
            while (enemy.CurrentHealth > 0 && Player.CurrentHealth > 0)
            {
                Console.WriteLine("Что вы будете делать?");
                Console.WriteLine("1. Ударить");
                Console.WriteLine("2. Пропустить ход");
                Console.WriteLine("3. Использовать аптечку");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Player.DealDamage(enemy);
                        if (enemy.CurrentHealth > 0)
                        {
                            enemy.Attack(Player);
                        }
                        break;
                    case "2":
                        Console.WriteLine($"{Player.Name} пропустил ход.");
                        enemy.Attack(Player);
                        break;
                    case "3":
                        Player.UseAid();
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
                Console.WriteLine($"У противника {enemy.CurrentHealth}hp, у вас {Player.CurrentHealth}hp\n");
            }
            if (Player.CurrentHealth <= 0)
            {
                Console.WriteLine("Вы погибли. Игра окончена.");
            }
            else
            {
                Console.WriteLine($"Вы победили {enemy.Name}! Получаете 10 очков.");
                Player.Score += 10;
            }
        }
    }
    private Enemy GenerateRandomEnemy()
    {
        Random rand = new Random();
        string name = $"Варвар {rand.Next(1, 200)}";
        int maxHealth = rand.Next(50, 200);
        Weapon enemyWeapon = new Weapon($"Оружие {rand.Next(1, 100)}", rand.Next(10, 25), 10);
        return new Enemy(name, maxHealth, enemyWeapon);
    }
}
public class Player
{
    public string Name { get; private set; }
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }
    public Aid AidKit { get; private set; }
    public Weapon Weapon { get; private set; }
    public int Score { get; set; }
    public Player(string name, int maxHealth, Aid aidKit, Weapon weapon)
    {
        Name = name;
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        AidKit = aidKit;
        Weapon = weapon;
        Score = 0;
    }
    public void DealDamage(Enemy enemy)
    {
        enemy.TakeDamage(Weapon.Damage);
        Weapon.Use();
    }
    public void UseAid()
    {
        if (AidKit != null)
        {
            CurrentHealth += AidKit.HealAmount;
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
            Console.WriteLine($"{Name} использовал аптечку. У вас {CurrentHealth}hp.");
            AidKit = null;
        }
        else
        {
            Console.WriteLine("Аптечка уже использована.");
        }
    }
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth < 0) CurrentHealth = 0;
        Console.WriteLine($"{Name} получил {damage} урона. У вас {CurrentHealth}hp.");
    }
}
public class Enemy
{
    public string Name { get; private set; }
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }
    public Weapon Weapon { get; private set; }
    public Enemy(string name, int maxHealth, Weapon weapon)
    {
        Name = name;
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        Weapon = weapon;
    }
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth < 0) CurrentHealth = 0;
        Console.WriteLine($"{Name} получил {damage} урона. У него {CurrentHealth}hp.");
    }
    public void Attack(Player player)
    {
        player.TakeDamage(Weapon.Damage);
        Console.WriteLine($"{Name} атакует {player.Name} на {Weapon.Damage} урона.");
    }
}
public class Aid
{
    public string Name { get; private set; }
    public int HealAmount { get; private set; }
    public Aid(string name, int healAmount)
    {
        Name = name;
        HealAmount = healAmount;
    }
}
public class Weapon
{
    public string Name { get; private set; }
    public int Damage { get; private set; }
    public int Durability { get; private set; }
    public Weapon(string name, int damage, int durability)
    {
        Name = name;
        Damage = damage;
        Durability = durability;
    }
    public void Use()
    {
        if (Durability > 0)
        {
            Durability--;
        }
        else
        {
            Console.WriteLine($"{Name} сломано!");
        }
    }
}
