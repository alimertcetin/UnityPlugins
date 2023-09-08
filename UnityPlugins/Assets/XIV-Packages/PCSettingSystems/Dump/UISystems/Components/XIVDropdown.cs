using System.Collections.Generic;

namespace XIV_Packages.PCSettingSystems
{
    [System.Serializable]
    public struct DropdownOptionData<T>
    {
        public T value;

        public DropdownOptionData(T value)
        {
            this.value = value;
        }
    }

    public abstract class XIVDropdown<T> : TMPro.TMP_Dropdown
    {
        List<DropdownOptionData<T>> dropdownOptionDatas = new List<DropdownOptionData<T>>();

        [System.Obsolete("Use AddOption, RemoveOption methods instead", true)]
        public new List<OptionData> options => base.options;

        public void SetSelectedIndexForData(T data)
        {
            int index = IndexOf(data);
            value = index;
        }

        public void SetSelectedIndexForDataWithoutNotify(T data)
        {
            int index = IndexOf(data);
            SetValueWithoutNotify(index);
        }

        public new void ClearOptions()
        {
            base.ClearOptions();
            dropdownOptionDatas.Clear();
        }

        public int IndexOf(T data)
        {
            for (int i = 0; i < dropdownOptionDatas.Count; i++)
            {
                if (dropdownOptionDatas[i].value.Equals(data))
                {
                    return i;
                }
            }

            return -1;
        }

        public DropdownOptionData<T> GetData(int index)
        {
            return dropdownOptionDatas[index];
        }

        public void BindData(int optionDataIndex, T data)
        {
            dropdownOptionDatas[optionDataIndex] = new DropdownOptionData<T>(data);
        }

        public void AddOption(OptionData optionData, DropdownOptionData<T> data, bool refreshImmediate = false)
        {
            base.options.Add(optionData);
            if (refreshImmediate) base.RefreshShownValue();
            dropdownOptionDatas.Add(data);
        }

        public new void AddOptions(List<OptionData> optionDatas)
        {
            AddOptions((IList<OptionData>)optionDatas);
        }

        public void AddOptions(IList<OptionData> optionDatas)
        {
            base.options.AddRange(optionDatas);
            base.RefreshShownValue();

            int count = optionDatas.Count;
            for (int i = 0; i < count; i++)
            {
                dropdownOptionDatas.Add(default);
            }
        }

        public void AddOptions(IList<OptionData> resolutionOptions, IList<DropdownOptionData<T>> optionDatas)
        {
            base.options.AddRange(resolutionOptions);
            dropdownOptionDatas.AddRange(optionDatas);
            base.RefreshShownValue();
        }

        void RemoveOption(OptionData optionData)
        {
            base.options.Remove(optionData);
        }

        void RemoveOption(T data)
        {
            int index = IndexOf(data);
            base.options.RemoveAt(index);
            dropdownOptionDatas.RemoveAt(index);
        }
    }
}