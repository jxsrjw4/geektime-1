using System;
using System.Collections.Generic;
using System.Text;

namespace GeekTime.Domain.MenuAggregate
{
    public class Menu:Entity<string>, IAggregateRoot
    {
        public string MenuName { get; private set; }

        public List<MenuItem> MenuItemList { get; private set; }

        public Menu() 
        {
            MenuName = "";
            MenuItemList = new List<MenuItem>();
        }
    }
}
