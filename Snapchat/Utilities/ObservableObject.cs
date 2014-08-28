﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SnapDotNet.Utilities
{
	/// <summary>
	/// Provides a base implementation of <see cref="INotifyPropertyChanged"/> allowing properties
	/// of derived classes to be observed for changes.
	/// </summary>
	public abstract class ObservableObject
		: INotifyPropertyChanged
	{
		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		/// <summary>
		/// Assigns the specified <paramref name="value"/> to the specified <paramref name="field"/>
		/// reference, raising the <see cref="PropertyChanged"/> event if the value has been changed.
		/// </summary>
		/// <typeparam name="T">The type of the value.</typeparam>
		/// <param name="field">The reference to a field.</param>
		/// <param name="value">The value to assign to the field.</param>
		/// <param name="suppressEqualityCheck">
		/// If <c>true</c>, always raise the <see cref="PropertyChanged"/> event regardless of whether
		/// the value remains the same before and after this method's invocation.
		/// </param>
		/// <param name="propertyName">
		/// The name of the property. Leave as default to use the member name of the caller of this method.
		/// </param>
		/// <returns>
		/// A boolean value indicating whether the <see cref="PropertyChanged"/> event was raised.
		/// </returns>
		protected bool SetValue<T>(ref T field, T value, bool suppressEqualityCheck = false, [CallerMemberName] string propertyName = "")
		{
			if (!suppressEqualityCheck && EqualityComparer<T>.Default.Equals(field, value))
				return false;

			field = value;
			OnPropertyChanged(propertyName);

			return true;
		}

		/// <summary>
		/// Raises the <see cref="PropertyChanged"/> event.
		/// </summary>
		/// <param name="propertyName">
		/// The name of the property. Leave as default to use the member name of the caller of this method.
		/// </param>
		protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			// Enable this if you HAVE to. It's spams the shit out of the output.
			//Debug.WriteLine("[INPC] Property Changed Event Fired under {0}", propertyName);

			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>
		/// Calls the <see cref="OnPropertyChanged" /> method when items inside an observable collection are modified.
		/// </summary>
		/// <param name="e">The CollectionChanged event of the Observable Collection.</param>
		/// <param name="collectionName">The name of the observable collection.</param>
		/// <param name="optionalPostAction">An optional action to also fire on propertu changed.</param>
		protected void OnObservableCollectionChanged(NotifyCollectionChangedEventArgs e, string collectionName, Action optionalPostAction = null)
		{
			if (optionalPostAction == null)
				optionalPostAction = delegate { };

			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Remove:
					foreach (ObservableObject item in e.OldItems)
						item.PropertyChanged -= delegate { OnPropertyChanged(collectionName); optionalPostAction(); }; // Removed items
					break;
				case NotifyCollectionChangedAction.Add:
					foreach (ObservableObject item in e.NewItems)
						item.PropertyChanged += delegate { OnPropertyChanged(collectionName); optionalPostAction(); }; // Added items
					break;
			}
		}
	}
}
