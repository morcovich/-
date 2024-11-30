using System.Security.Cryptography;
using System;

namespace рогалик
{
    public class Player
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public Aid MedKit { get; set; }
        public Weapon Weapon { get; set; }
        public int Score { get; set; }

        public Player(string name)
        {
            Name = name;
            MaxHealth = 100;
            Health = MaxHealth;
            Score = 0;
        }

        public void UseMedKit()
        {
            if (MedKit != null)
            {
                Health = Math.Min(Health + MedKit.HealAmount, MaxHealth);
                Console.WriteLine($"{Name} использовал аптечку. Ваше здоровье: {Health}/{MaxHealth}");
            }
            else
            {
                Console.WriteLine("У вас нет аптечек!");
            }
        }

        public void Attack(Enemy enemy)
        {
            if (Weapon != null)
            {
                Console.WriteLine($"{Name} ударил {enemy.Name} с оружием {Weapon.Name}!");
                enemy.TakeDamage(Weapon.Damage);
            }
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Health = 0;
                Console.WriteLine($"{Name} умер!");
            }
        }
    }
    public class Enemy
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public Weapon Weapon { get; set; }

        public Enemy(string name)
        {
            Name = name;
            MaxHealth = 50;
            Health = MaxHealth;
        }

        public void Attack(Player player)
        {
            if (Weapon != null)
            {
                Console.WriteLine($"{Name} ударил {player.Name} с оружием {Weapon.Name}!");
                player.TakeDamage(Weapon.Damage);
            }
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Health = 0;
                Console.WriteLine($"{Name} побежден!");
            }
        }
    }
    public class Aid
    {
        public string Name { get; set; }
        public int HealAmount { get; set; }

        public Aid(string name, int healAmount)
        {
            Name = name;
            HealAmount = healAmount;
        }
    }
    public class Weapon
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public int Durability { get; set; }

        public Weapon(string name, int damage, int durability)
        {
            Name = name;
            Damage = damage;
            Durability = durability;
        }

        public void Use()
        {
            Durability--;
            if (Durability <= 0)
            {
                Console.WriteLine($"Оружие {Name} сломалось!");
            }
        }
    }
   

public class Game
    {
        private static Random rnd = new Random();

        public static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать, воин!");
            Console.Write("Назови себя: ");
            string playerName = Console.ReadLine();

            Player player = new Player(playerName);

            // Генерация оружия и аптечек
            player.Weapon = GenerateWeapon();
            player.MedKit = GenerateMedKit();

            // Приветственное сообщение
            Console.WriteLine($"Ваше имя {player.Name}!");
            Console.WriteLine($"Вам был ниспослан {player.Weapon.Name} ({player.Weapon.Damage})");
            Console.WriteLine($"А также {player.MedKit.Name} ({player.MedKit.HealAmount}hp).");
            Console.WriteLine($"У вас {player.Health}hp.");

            // Главный игровой цикл
            while (player.Health > 0)
            {
                Enemy enemy = GenerateEnemy();
                Console.WriteLine($"\n**{player.Name}** встречает врага **{enemy.Name} ({enemy.Health}hp)**");
                Console.WriteLine($"У врага на поясе сияет оружие **{enemy.Weapon.Name} ({enemy.Weapon.Damage})**");

                // Боевой цикл
                while (enemy.Health > 0 && player.Health > 0)
                {
                    Console.WriteLine("Что вы будете делать?");
                    Console.WriteLine("1. Ударить");
                    Console.WriteLine("2. Пропустить ход");
                    Console.WriteLine("3. Использовать аптечку");

                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            player.Attack(enemy);
                            break;
                        case "2":
                            Console.WriteLine($"{player.Name} пропустил ход.");
                            break;
                        case "3":
                            player.UseMedKit();
                            break;
                        default:
                            Console.WriteLine("Неверный выбор.");
                            break;
                    }

                    if (enemy.Health > 0)
                    {
                        enemy.Attack(player);
                    }
                }

                // Если игрок победил
                if (enemy.Health <= 0)
                {
                    player.Score += 10;
                    Console.WriteLine($"Вы победили! Ваши очки: {player.Score}");
                }

                if (player.Health <= 0)
                {
                    Console.WriteLine("Игра окончена!");
                }
            }
        }

        // Генерация случайного врага
        private static Enemy GenerateEnemy()
        {
            string[] enemyNames = { "Варвар", "Гоблин", "Дракон", "Орк" };
            string name = enemyNames[rnd.Next(enemyNames.Length)];
            Enemy enemy = new Enemy(name)
            {
                Weapon = GenerateWeapon()
            };
            return enemy;
        }

        // Генерация случайного оружия
        private static Weapon GenerateWeapon()
        {
            string[] weaponNames = { "Меч", "Лук", "Дробовик", "Экскалибур" };
            int damage = rnd.Next(5, 21);
            int durability = rnd.Next(1, 6);
            return new Weapon(weaponNames[rnd.Next(weaponNames.Length)], damage, durability);
        }

        // Генерация случайной аптечки
        private static Aid GenerateMedKit()
        {
            string[] medKitNames = { "Маленькая аптечка", "Средняя аптечка", "Большая аптечка" };
            int healAmount = rnd.Next(5, 21);
            return new Aid(medKitNames[rnd.Next(medKitNames.Length)], healAmount);
        }
    }




}
