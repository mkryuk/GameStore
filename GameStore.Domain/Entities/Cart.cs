using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Domain.Entities
{
    public class Cart
    {
        public List<CartLine> Lines { get; private set; } = new List<CartLine>();
        public void AddItem(Game game, int quantity)
        {
            var item = Lines.FirstOrDefault(it => it.Game.GameId == game.GameId);
            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                Lines.Add(new CartLine
                {
                    Game = game,
                    Quantity = quantity
                });
            }            
        }

        public void RemoveLine(Game game)
        {
            Lines.RemoveAll(it => it.Game.GameId == game.GameId);
        }

        public decimal ComputeTotalValue()
        {
            return Lines.Sum(it => it.Game.Price * it.Quantity);
        }

        public void Clear()
        {
            Lines.Clear();
        }

        public class CartLine
        {
            public Game Game { get; set; }
            public int Quantity { get; set; }
        }
    }
}
