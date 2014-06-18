﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Snapchat.Controls
{
	public sealed class ExpanderView : ContentControl
	{
		public static DependencyProperty HeaderTextDependencyProperty;
		public static DependencyProperty SubHeaderTextDependencyProperty;
		public static DependencyProperty ShowSubHeaderDependencyProperty;
		public static DependencyProperty ExpandableContentTemplateDependencyProperty;
		public static DependencyProperty HeaderHeightDependencyProperty;
		public static DependencyProperty ContentHeightDependencyProperty;

		public bool IsExpanded { get; private set; }

		#region Initalizers

		/// <summary>
		/// 
		/// </summary>
		static ExpanderView()
		{
			HeaderTextDependencyProperty = DependencyProperty.Register("HeaderText", typeof(String), typeof(ExpanderView), new PropertyMetadata(""));
			SubHeaderTextDependencyProperty = DependencyProperty.Register("SubHeaderText", typeof(String), typeof(ExpanderView), new PropertyMetadata(""));
			ShowSubHeaderDependencyProperty = DependencyProperty.Register("ShowSubHeader", typeof(Boolean), typeof(ExpanderView), new PropertyMetadata(false));

			ExpandableContentTemplateDependencyProperty = DependencyProperty.Register("ExpandableContentTemplate", typeof(DataTemplate), typeof(ExpanderView), new PropertyMetadata(null));

			HeaderHeightDependencyProperty = DependencyProperty.Register("HeaderHeight", typeof(int), typeof(ExpanderView), new PropertyMetadata(60));
			ContentHeightDependencyProperty = DependencyProperty.Register("ContentHeight", typeof(int), typeof(ExpanderView), new PropertyMetadata(70));
		}

		/// <summary>
		/// 
		/// </summary>
		public ExpanderView()
		{
			IsExpanded = false;

			Tapped += (sender, args) =>
			{
				if (IsExpanded)
					Contract();
				else
					Expand();
			};
		}

		#endregion

		#region Public Methods

		public void Contract()
		{
			if (!IsExpanded) return;

			if (Contracted != null)
				Contracted(this, true);
			VisualStateManager.GoToState(this, "Contracted", true);
			IsExpanded = false;
		}

		public void Expand()
		{
			if (IsExpanded) return;

			if (Expanded != null)
				Expanded(this, true);
			VisualStateManager.GoToState(this, "Expanded", true);
			IsExpanded = true;
		}

		#endregion

		#region Events

		public event EventHandler<Boolean> Expanded;
		public event EventHandler<Boolean> Contracted;

		#endregion

		#region Overrides

		protected override void OnApplyTemplate()
		{
			VisualStateManager.GoToState(this, "Contracted", true);
		}

		#endregion

		#region Getters/Setters

		/// <summary>
		/// 
		/// </summary>
		public String HeaderText
		{
			get { return (String) GetValue(HeaderTextDependencyProperty); }
			set { SetValue(HeaderTextDependencyProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public String SubHeaderText
		{
			get { return (String) GetValue(SubHeaderTextDependencyProperty); }
			set { SetValue(SubHeaderTextDependencyProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public Boolean ShowSubHeader
		{
			get { return (Boolean) GetValue(ShowSubHeaderDependencyProperty); }
			set { SetValue(ShowSubHeaderDependencyProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public DataTemplate ExpandableContentTemplate
		{
			get { return (DataTemplate) GetValue(ExpandableContentTemplateDependencyProperty); }
			set { SetValue(ExpandableContentTemplateDependencyProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public int HeaderHeight
		{
			get { return (int) GetValue(HeaderHeightDependencyProperty); }
			set { SetValue(HeaderHeightDependencyProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public int ContentHeight
		{
			get { return (int) GetValue(ContentHeightDependencyProperty); }
			set { SetValue(ContentHeightDependencyProperty, value); }
		}

		#endregion
	}
}
