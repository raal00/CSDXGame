using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXFormHandler.Models.Objects
{
    public enum ObjectTypeEnum
    {
        Terrain = 0, // {glass(terrain), roads etc} Texture
        Environment = 1,  // {tree, rock, grass(object)} Objects
        NPC = 2, // Objects
        ControlledNPC = 3, // Objects
        Hero = 4, // Object
    }
}
