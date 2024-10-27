using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;

namespace SCADACreator
{
    public class ItemEvent
    {
        private ItemEventType eventType;

        public ItemEvent()
        {

        }

       public enum ItemEventType
        {
            emClick,
            emPress,
            emRelease
        }

        public enum ItemActiontype
        {
            emSetbit,
            emResetBit
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public ItemEventType EventType { get => eventType; set => eventType = value; }
        public ItemActiontype ActionType { get; set; }
        public virtual TagInfo Tag { get; set; }

    }
}
