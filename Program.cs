using ImageProcessor;

namespace XS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (!Directory.Exists("output"))
            {
                Directory.CreateDirectory("output");
            }

            if (args.Length == 0)
            {
                GenerateVariants();
            }
            else
            {
                switch (args[0])
                {
                    case "generateVariants":
                        GenerateVariants();
                        break;
                    
                    case "generateColors":
                        GenerateColorVariants();
                        break;

                    default:
                        Console.WriteLine($"Unknown operation '{args[0]}' requested");
                        break;
                }
            }
        }

        public static void GenerateVariants()
        {
            var directory1 = Directory.GetFiles("output");

            foreach (var i in directory1)
            {
                genVariants(i, true);
            }

            // Do it again for dye, this allows for objects to have both wood and dye variants
            var directory2 = Directory.GetFiles("output");

            foreach (var i in directory2)
            {
                genVariants(i, false);
            }
        }

        public static void GenerateColorVariants()
        {
            var dir = Directory.GetFiles("output");

            foreach (var i in dir)
            {
                if (i.EndsWith(".png"))
                {
                    genColors(i);
                }
            }
        }

        public static void genVariants(string path, bool wood)
        {
            var file = File.OpenText(path);
            var text = file.ReadToEnd();

            string replacing;
            string[] needed;

            if (wood)
            {
                replacing = "oak";
                needed = new string[] { "birch", "spruce", "jungle", "acacia", "dark_oak", "crimson", "warped" };
            }
            else
            {
                replacing = "white";
                needed = new string[] {
                    "orange",
                    "magenta",
                    "light_blue",
                    "yellow",
                    "lime",
                    "pink",
                    "gray",
                    "light_gray",
                    "cyan",
                    "purple",
                    "blue",
                    "brown",
                    "green",
                    "red",
                    "black"
                };
            }

            for (int i = 0; i < needed.Length; i++)
            {
                var new_filename = path.Replace(replacing, needed[i]);

                if (File.Exists(new_filename))
                {
                    continue;
                }

                var newtext = text.Replace(replacing, needed[i]);

                File.WriteAllText(path.Replace(replacing, needed[i]), newtext);
            }
        }

        public static void genColors(string path)
        {
            Dictionary<string, string> ColorValues = new Dictionary<string, string>
            {
                { "black", "#1D1D21" },
                { "red", "#B02E26" },
                { "green", "#5E7C16" },
                { "brown", "#835432" },
                { "blue", "#3C44AA" },
                { "purple", "#8932B8" },
                { "cyan", "#169C9C" },
                { "light_gray", "#9D9D97" },
                { "gray", "#474F52" },
                { "pink", "#F38BAA" },
                { "lime", "#80C71F" },
                { "yellow", "#FED83D" },
                { "light_blue", "#3AB3DA" },
                { "magenta", "#C74EBD" },
                { "orange", "#F9801D" }
            };

            foreach (var i in ColorValues.Keys)
            {
                using (ImageFactory factory = new ImageFactory())
                {
                    factory.Load(path)
                        .Tint(System.Drawing.ColorTranslator.FromHtml(ColorValues[i]))
                        .Save(path.Replace("white", i));
                }
            }
        }
    }
}