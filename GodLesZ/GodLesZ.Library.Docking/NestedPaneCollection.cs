using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace GodLesZ.Library.Docking {
	public sealed class NestedPaneCollection : ReadOnlyCollection<DockPane> {
		private INestedPanesContainer m_container;
		private VisibleNestedPaneCollection m_visibleNestedPanes;

		internal NestedPaneCollection(INestedPanesContainer container)
			: base(new List<DockPane>()) {
			m_container = container;
			m_visibleNestedPanes = new VisibleNestedPaneCollection(this);
		}

		public INestedPanesContainer Container {
			get { return m_container; }
		}

		public VisibleNestedPaneCollection VisibleNestedPanes {
			get { return m_visibleNestedPanes; }
		}

		public DockState DockState {
			get { return Container.DockState; }
		}

		public bool IsFloat {
			get { return DockState == DockState.Float; }
		}

		internal void Add(DockPane pane) {
			if (pane == null)
				return;

			NestedPaneCollection oldNestedPanes = (pane.NestedPanesContainer == null) ? null : pane.NestedPanesContainer.NestedPanes;
			if (oldNestedPanes != null)
				oldNestedPanes.InternalRemove(pane);
			Items.Add(pane);
			if (oldNestedPanes != null)
				oldNestedPanes.CheckFloatWindowDispose();
		}

		private void CheckFloatWindowDispose() {
			if (Count == 0 && Container.DockState == DockState.Float) {
				FloatWindow floatWindow = (FloatWindow)Container;
				if (!floatWindow.Disposing && !floatWindow.IsDisposed)
					NativeMethods.PostMessage(((FloatWindow)Container).Handle, FloatWindow.WM_CHECKDISPOSE, 0, 0);
			}
		}

		internal void Remove(DockPane pane) {
			InternalRemove(pane);
			CheckFloatWindowDispose();
		}

		private void InternalRemove(DockPane pane) {
			if (!Contains(pane))
				return;

			NestedDockingStatus statusPane = pane.NestedDockingStatus;
			DockPane lastNestedPane = null;
			for (int i = Count - 1; i > IndexOf(pane); i--) {
				if (this[i].NestedDockingStatus.PreviousPane == pane) {
					lastNestedPane = this[i];
					break;
				}
			}

			if (lastNestedPane != null) {
				int indexLastNestedPane = IndexOf(lastNestedPane);
				Items.Remove(lastNestedPane);
				Items[IndexOf(pane)] = lastNestedPane;
				NestedDockingStatus lastNestedDock = lastNestedPane.NestedDockingStatus;
				lastNestedDock.SetStatus(this, statusPane.PreviousPane, statusPane.Alignment, statusPane.Proportion);
				for (int i = indexLastNestedPane - 1; i > IndexOf(lastNestedPane); i--) {
					NestedDockingStatus status = this[i].NestedDockingStatus;
					if (status.PreviousPane == pane)
						status.SetStatus(this, lastNestedPane, status.Alignment, status.Proportion);
				}
			} else
				Items.Remove(pane);

			statusPane.SetStatus(null, null, DockAlignment.Left, 0.5);
			statusPane.SetDisplayingStatus(false, null, DockAlignment.Left, 0.5);
			statusPane.SetDisplayingBounds(Rectangle.Empty, Rectangle.Empty, Rectangle.Empty);
		}

		public DockPane GetDefaultPreviousPane(DockPane pane) {
			for (int i = Count - 1; i >= 0; i--)
				if (this[i] != pane)
					return this[i];

			return null;
		}
	}
}
