using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using PowerPointLabs.ColorsLab;

namespace PowerPointLabs.DataSources
{
    public class ColorDataSource : INotifyPropertyChanged
    {
        public IList<HSLColor> RecentColors
        {
            get
            {
                return recentColors;
            }
        }

        public IList<HSLColor> FavoriteColors
        {
            get
            {
                return favoriteColors;
            }
        }

        public readonly int NumRecentColorFields = 12;
        public readonly int NumFavoriteColorFields = 12;

        private ObservableCollection<HSLColor> recentColors;
        private ObservableCollection<HSLColor> favoriteColors;

        #region Properties

        private HSLColor selectedColorValue;

        public HSLColor SelectedColor
        {       
            get
            {
                return selectedColorValue;
            }
            set
            {
                if (value != selectedColorValue)
                {
                    selectedColorValue = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("selectedColor"));
                }
            }
        }

        #endregion

        #region Constructors

        public ColorDataSource()
        {
            recentColors = new ObservableCollection<HSLColor>();
            favoriteColors = new ObservableCollection<HSLColor>();
            recentColors.CollectionChanged += RecentColors_CollectionChanged;
            favoriteColors.CollectionChanged += FavoriteColors_CollectionChanged;
            ClearRecentColors();
            ClearFavoriteColors();
        }

        #endregion

        #region API

        public void AddColorToRecentColors(HSLColor color)
        {
            int index = recentColors.IndexOf(color);
            if (index == -1)
            {
                index = recentColors.Count - 1;
            }

            for (int i = index; i > 0; i--)
            {
                recentColors[i] = recentColors[i - 1];
            }
            recentColors[0] = color;
        }

        public void AddColorToFavorites(HSLColor color)
        {
            for (int i = favoriteColors.Count - 1; i > 0; i--)
            {
                favoriteColors[i] = favoriteColors[i - 1];
            }
            favoriteColors[0] = color;
        }

        public void ClearRecentColors()
        {
            recentColors.Clear();
            for (int i = 0; i < NumRecentColorFields; i++)
            {
                recentColors.Add(Color.White);
            }
        }

        public void ClearFavoriteColors()
        {
            favoriteColors.Clear();
            for (int i = 0; i < NumFavoriteColorFields; i++)
            {
                favoriteColors.Add(Color.White);
            }
        }

        public ObservableCollection<HSLColor> GetListOfRecentColors()
        {
            return recentColors;
        }

        public ObservableCollection<HSLColor> GetListOfFavoriteColors()
        {
            return favoriteColors;
        }

        public void SetRecentColor(int index, HSLColor color)
        {
            if (index >= recentColors.Count)
            {
                return;
            }
            recentColors[index] = color;
        }

        public void SetFavoriteColor(int index, HSLColor color)
        {
            if (index >= favoriteColors.Count)
            {
                return;
            }
            favoriteColors[index] = color;
        }

        #endregion

        #region Save/Load Colors

        public bool SaveRecentColorsInFile(string filePath)
        {
            try
            {
                Stream fileStream = File.Create(filePath);
                BinaryFormatter serializer = new BinaryFormatter();
                HSLColor[] colors = new HSLColor[recentColors.Count];
                recentColors.CopyTo(colors, 0);
                List<HSLColor> colorList = new List<HSLColor>(colors);
                serializer.Serialize(fileStream, colorList);
                fileStream.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool LoadRecentColorsFromFile(string filePath)
        {
            try
            {
                Stream openFileStream = File.OpenRead(filePath);
                BinaryFormatter deserializer = new BinaryFormatter();
                List<HSLColor> newRecentColors = (List<HSLColor>)deserializer.Deserialize(openFileStream);
                openFileStream.Close();

                recentColors.Clear();
                foreach (HSLColor recentColor in newRecentColors)
                {
                    recentColors.Add(recentColor);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool SaveFavoriteColorsInFile(String filePath)
        {
            try
            {
                Stream fileStream = File.Create(filePath);
                BinaryFormatter serializer = new BinaryFormatter();
                HSLColor[] colors = new HSLColor[favoriteColors.Count];
                favoriteColors.CopyTo(colors, 0);
                List<HSLColor> colorList = new List<HSLColor>(colors);
                serializer.Serialize(fileStream, colorList);
                fileStream.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool LoadFavoriteColorsFromFile(string filePath)
        {
            try
            {
                Stream openFileStream = File.OpenRead(filePath);
                BinaryFormatter deserializer = new BinaryFormatter();
                List<HSLColor> newFavoriteColors = (List<HSLColor>)deserializer.Deserialize(openFileStream);
                openFileStream.Close();

                favoriteColors.Clear();
                foreach (HSLColor favoriteColor in newFavoriteColors)
                {
                    favoriteColors.Add(favoriteColor);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region EventHandlers

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property. 
        // The CallerMemberName attribute that is applied to the optional propertyName 
        // parameter causes the property name of the caller to be substituted as an argument. 
        // Create the OnPropertyChanged method to raise the event
        protected void RecentColors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RecentColors)));
        }

        protected void FavoriteColors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FavoriteColors)));
        }

        #endregion

    }
}
