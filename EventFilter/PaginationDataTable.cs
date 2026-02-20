using EventFilter.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventFilter
{
    public class PaginationDataTable : DataTable
    {
        private int _pageSize;
        private int _currentPage;
        private int _totalPages;
        private int _totalRecords;
        private int _startRecord;
        private int _endRecord;
        public delegate void UpdateResultText(string text);

        public event PaginationDataTable.UpdateResultText UpdateText;

        /**
         * Source type = false if from search, true if from filter
         **/
        private bool sourceType = false;

        public PaginationDataTable(int pageSize, bool sourceType = false)
        {
            _pageSize = pageSize;
            _currentPage = 1;
            this.sourceType = sourceType;
        }

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        public int CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; }
        }

        public int TotalPages
        {
            get { return _totalPages; }
            set { _totalPages = value; }
        }

        public int TotalRecords
        {
            get { return _totalRecords; }
            set { _totalRecords = value; }
        }

        public int StartRecord
        {
            get { return _startRecord; }
            set { _startRecord = value; }
        }

        public int EndRecord
        {
            get { return _endRecord; }
            set { _endRecord = value; }
        }

        private void SetPaging()
        {
            StartRecord = (CurrentPage - 1) * PageSize + 1;
            EndRecord = Math.Min(CurrentPage * PageSize, TotalRecords);

            base.Rows.Clear();

            if (sourceType)
            {
                var newEvents = Event.GetInstance().GetFilteredEvents().Skip(StartRecord);

                foreach (var x in newEvents)
                {
                    base.Rows.Add(x.Item2.Date.ToString(), x.Item2.Description, x.Item2.Id, x.Item1);
                }
            }
            else
            {
                var newEvents = Event.GetInstance().GetFoundEvents().Skip(StartRecord);

                foreach (var x in newEvents)
                {
                    base.Rows.Add(x.Date.ToString(), x.Description, x.Id);
                }
            }

            UpdateText?.Invoke(GetPagingInfo());
        }

        //public void SetPaging(int totalRecords)
        //{
        //    TotalRecords = totalRecords;
        //    TotalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);
        //    StartRecord = (CurrentPage - 1) * PageSize + 1;
        //    EndRecord = Math.Min(CurrentPage * PageSize, TotalRecords);
        //}

        //public void SetPaging(int totalRecords, int pageSize)
        //{
        //    PageSize = pageSize;
        //    SetPaging(totalRecords);
        //}

        //public void SetPaging(int totalRecords, int pageSize, int currentPage)
        //{
        //    CurrentPage = currentPage;
        //    SetPaging(totalRecords, pageSize);
        //}

        //public void SetPaging(int totalRecords, int pageSize, int currentPage, int totalPages)
        //{
        //    TotalPages = totalPages;
        //    SetPaging(totalRecords, pageSize, currentPage);
        //}

        //public void SetPaging(int totalRecords, int pageSize, int currentPage, int totalPages, int startRecord, int endRecord)
        //{
        //    StartRecord = startRecord;
        //    EndRecord = endRecord;
        //    SetPaging(totalRecords, pageSize, currentPage, totalPages);
        //}

        public void NextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                SetPaging();
            }
        }

        public void PreviousPage()
        {
            if (CurrentPage > 0)
            {
                CurrentPage--;
                SetPaging();
            }
        }
        
        public string GetPagingInfo()
        {
            return $"Showing {StartRecord} to {EndRecord} of {TotalRecords} records";
        }

        public bool HasNextPage()
        {
            return CurrentPage < TotalPages;
        }

        public bool HasPreviousPage()
        {
            return CurrentPage > 1;
        }

        public void Add(params object[] values)
        {
            if (base.Rows.Count > PageSize)
            {
                UpdateInternalData();
                return;
            }

            base.Rows.Add(values);
            UpdateInternalData();
        }

        private void UpdateInternalData()
        {

            if (sourceType)
            {
                this._totalRecords = Event.GetInstance().GetFilteredEvents().Count();
            }
            else
            {
                this._totalRecords = Event.GetInstance().GetFoundEvents().Count();
            }

            this._startRecord = base.Rows[0].Field<int>("Id");
            this._endRecord = base.Rows[base.Rows.Count - 1].Field<int>("Id");
            this._totalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);
        }
    }
}
