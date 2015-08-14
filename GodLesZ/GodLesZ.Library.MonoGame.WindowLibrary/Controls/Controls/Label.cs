using Microsoft.Xna.Framework;

namespace GodLesZ.Library.Xna.WindowLibrary.Controls {

	public class Label : Control {

		private EAlignment alignment = EAlignment.MiddleLeft;
		private bool ellipsis = true;

		public virtual EAlignment Alignment {
			get { return alignment; }
			set { alignment = value; }
		}

		public virtual bool Ellipsis {
			get { return ellipsis; }
			set { ellipsis = value; }
		}

		public Label(Manager manager)
			: base(manager) {
			CanFocus = false;
			Passive = true;
			Width = 64;
			Height = 16;
		}


		public override void Init() {
			base.Init();
		}

		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime) {
			//base.DrawControl(renderer, rect, gameTime);

			SkinLayer s = new SkinLayer(Skin.Layers[0]);
			s.Text.Alignment = alignment;
			renderer.DrawString(this, s, Text, rect, true, 0, 0, ellipsis);
		}

	}

}
