using System.Collections.Generic;
using MedIn.Libs;

namespace MedIn.OziCms.PagesSettings.Forms
{
	public class UploadFileSettings : FieldSettings
	{
		#region edit settings

		/// <summary>
		/// файл должен быть картинкой
		/// </summary>
		public bool IsImage { get; set; }

		/// <summary>
		/// показывать заголовок файла для редактирования
		/// </summary>
		public bool ShowTitle { get; set; }

		/// <summary>
		/// показывать исходное имя файла для редактирования
		/// </summary>
		public bool ShowSourceName { get; set; }

		/// <summary>
		/// позволить сортировать файлы
		/// </summary>
		public bool Sortable { get; set; }

		/// <summary>
		/// редактировать видимость файла
		/// </summary>
		public bool Visibility { get; set; }

		/// <summary>
		/// показывать описание файла
		/// </summary>
		public bool ShowDescription { get; set; }

		/// <summary>
		/// показывать дату создания файла
		/// </summary>
		public bool ShowDate { get; set; }

		public List<WatermarkSettings> Watermarks { get; set; }

		#endregion

		#region admin view settings

		/// <summary>
		/// тип отображения (list - списком, tile - плиткой)
		/// </summary>
		public string DisplayType { get; set; }

		/// <summary>
		/// размер вывода картинок в админке
		/// </summary>
		public int Size { get; set; }

		/// <summary>
		/// относительный путь для сохранения
		/// </summary>
		public string PathToSave { get; set; }
		public override string Control { get { return "ozi_file"; } }

		#endregion
	}

	public class WatermarkSettings
	{
		/// <summary>
		/// относительный путь к водяному знаку
		/// </summary>
		public string RelativePath { get; set; }
		/// <summary>
		/// способ применения, либо позиция
		/// </summary>
		public WatermarkFillType FillType { get; set; }

		public int Left { get; set; }
		public int Top { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }

		/// <summary>
		/// поля
		/// </summary>
		public Margins Margins { get; set; }

		//// <summary>
		//// прозрачность
		//// </summary>
		//public double Opacity { get; set; }

		/// <summary>
		/// ширина, при которой ватермарк будет уменьшен
		/// </summary>
		public int? ReduceWidth { get; set; }

		/// <summary> 
		/// высота, при которой ватермарк будет уменьшен
		/// </summary>
		public int? ReduceHeight { get; set; }

		/// <summary>
		/// коэффициент уменьшения ватермарка
		/// </summary>
		public double ReduceFactor { get; set; }
	}

	public enum WatermarkFillType
	{
		/// <summary>
		/// не применяется ватермарк
		/// </summary>
		None,
		/// <summary>
		/// По указанным пользователем координатам
		/// </summary>
		Custom,
		/// <summary>
		/// Позиционируется в левый верхний угол
		/// </summary>
		TopLeft,
		/// <summary>
		/// Позиционируется в правый верхний угол
		/// </summary>
		TopRight,
		/// <summary>
		/// Позиционируется в левый нижний угол
		/// </summary>
		BottomLeft,
		/// <summary>
		/// Позиционируется в левый верхний угол
		/// </summary>
		BottomRight,
		/// <summary>
		/// Позиционируется по центру исходного изображения
		/// </summary>
		Center,
		/// <summary>
		/// заполняет всё исходное изображение
		/// </summary>
		Mosaic,
		/// <summary>
		/// растягивает watermark по всей поверхности исходного изображения
		/// </summary>
		Stretch
	}
}
