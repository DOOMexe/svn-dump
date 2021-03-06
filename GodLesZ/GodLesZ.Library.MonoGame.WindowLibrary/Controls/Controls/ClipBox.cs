

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GodLesZ.Library.Xna.WindowLibrary.Controls {

	public class ClipBox : Control {

		public ClipBox(Manager manager)
			: base(manager) {
			Color = Color.Transparent;
			BackColor = Color.Transparent;
			CanFocus = false;
			Passive = true;
		}

		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime) {
			base.DrawControl(renderer, rect, gameTime);
		}

	}

}