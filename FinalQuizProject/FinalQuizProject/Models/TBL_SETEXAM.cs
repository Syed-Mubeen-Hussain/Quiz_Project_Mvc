//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FinalQuizProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TBL_SETEXAM
    {
        public int EXAM_ID { get; set; }
        public System.DateTime EXAM_DATE { get; set; }
        public Nullable<int> EXAM_FK_STD { get; set; }
        public string EXAM_NAME { get; set; }
        public int EXAM_STD_SCORE { get; set; }
    
        public virtual TBL_STUDENT TBL_STUDENT { get; set; }
    }
}
