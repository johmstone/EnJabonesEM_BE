using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ET;
using DAL;

namespace BL
{
    public class IngredientsBL
    {
        private IngredientsDAL IDAL = new IngredientsDAL();

        public List<Ingredient> List ()
        {
            return IDAL.List();
        }

        public bool AddNew (Ingredient details, string InsertUser)
        {
            return IDAL.AddNew(details, InsertUser);
        }

        public List<Unit> Units()
        {
            return IDAL.Units();
        }

        public bool AddNewUnit(Unit details, string InsertUser)
        {
            return IDAL.AddNewUnit(details, InsertUser);
        }
    }
}
