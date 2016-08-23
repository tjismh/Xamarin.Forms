﻿using System;
using System.ComponentModel;
using AppKit;

namespace Xamarin.Forms.Platform.MacOS
{
	public class TextCellRenderer : CellRenderer
	{
		static readonly Color DefaultDetailColor = new Color(.32, .4, .57);
		static readonly Color DefaultTextColor = Color.Black;

		public override NSView GetCell(Cell item, NSView reusableView, NSTableView tv)
		{
			var textCell = (TextCell)item;

			var tvc = reusableView as CellTableViewCell;
			if (tvc == null)
				tvc = new CellTableViewCell(NSTableViewCellStyle.Subtitle, item.GetType().FullName);
			else
				tvc.Cell.PropertyChanged -= tvc.HandlePropertyChanged;

			tvc.Cell = textCell;
			textCell.PropertyChanged += tvc.HandlePropertyChanged;
			tvc.PropertyChanged = HandlePropertyChanged;

			tvc.TextLabel.StringValue = textCell.Text ?? "";
			tvc.DetailTextLabel.StringValue = textCell.Detail ?? "";
			tvc.TextLabel.TextColor = textCell.TextColor.ToNSColor(DefaultTextColor);
			tvc.DetailTextLabel.TextColor = textCell.DetailColor.ToNSColor(DefaultDetailColor);

			WireUpForceUpdateSizeRequested(item, tvc, tv);

			UpdateIsEnabled(tvc, textCell);

			UpdateBackground(tvc, item);

			return tvc;
		}

		protected virtual void HandlePropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			var tvc = (CellTableViewCell)sender;
			var textCell = (TextCell)tvc.Cell;
			if (args.PropertyName == TextCell.TextProperty.PropertyName)
			{
				tvc.TextLabel.StringValue = ((TextCell)tvc.Cell).Text;
				tvc.TextLabel.SizeToFit();
			}
			else if (args.PropertyName == TextCell.DetailProperty.PropertyName)
			{
				tvc.DetailTextLabel.StringValue = ((TextCell)tvc.Cell).Detail;
				tvc.DetailTextLabel.SizeToFit();
			}
			else if (args.PropertyName == TextCell.TextColorProperty.PropertyName)
				tvc.TextLabel.TextColor = textCell.TextColor.ToNSColor(DefaultTextColor);
			else if (args.PropertyName == TextCell.DetailColorProperty.PropertyName)
				tvc.DetailTextLabel.TextColor = textCell.DetailColor.ToNSColor(DefaultTextColor);
			else if (args.PropertyName == Cell.IsEnabledProperty.PropertyName)
				UpdateIsEnabled(tvc, textCell);
		}

		static void UpdateIsEnabled(CellTableViewCell cell, TextCell entryCell)
		{
			cell.TextLabel.Enabled = entryCell.IsEnabled;
			cell.DetailTextLabel.Enabled = entryCell.IsEnabled;
		}
	}
}

