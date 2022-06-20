using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using ViconDataStreamSDK.CSharp;




namespace UnityVicon
{
  public class SubjectScript : MonoBehaviour
  {
    public string SubjectName = "";
    
    private bool IsScaled = true;

    public ViconDataStreamClient Client;

    private KalmanFilterVector3 _filteredPos;
    private KalmanFilterVector4 _filteredRot;

        private AdaptiveDoubleExponentialFilterVector3 _adaptivePos;
        private AdaptiveDoubleExponentialFilterVector4 _adaptiveRot;

        public void Start()
        {
            _filteredPos = new KalmanFilterVector3();
            _filteredRot = new KalmanFilterVector4();
            _adaptivePos = new AdaptiveDoubleExponentialFilterVector3();
            _adaptiveRot = new AdaptiveDoubleExponentialFilterVector4();
        }

        public SubjectScript()
    {
    }

    void LateUpdate()
    {
      Output_GetSubjectRootSegmentName OGSRSN = Client.GetSubjectRootSegmentName(SubjectName);
      Transform Root = transform.root;
      FindAndTransform( Root, OGSRSN.SegmentName);
    }

    string strip(string BoneName)
    {
      if (BoneName.Contains(":"))
      {
        string[] results = BoneName.Split(':');
        return results[1];
      }
      return BoneName;
    }
    void FindAndTransform(Transform iTransform, string BoneName )
    {
      int ChildCount = iTransform.childCount;
      for (int i = 0; i < ChildCount; ++i)
      {
        Transform Child = iTransform.GetChild(i);
        if( strip( Child.name) == BoneName )
        { 
          ApplyBoneTransform(Child);
          TransformChildren(Child);
          break;
        }
        // if not finding root in this layer, try the children
        FindAndTransform(Child, BoneName);
      }
    }
    void TransformChildren(Transform iTransform )
    {
      int ChildCount = iTransform.childCount;
      for (int i = 0; i < ChildCount; ++i)
      {
        Transform Child = iTransform.GetChild(i);
        ApplyBoneTransform(Child);
        TransformChildren(Child);
      }
    }
      // map the orientation back for forward

    private void ApplyBoneTransform(Transform Bone )
    {
      string BoneName = strip(Bone.gameObject.name);
      // update the bone transform from the data stream
      Output_GetSegmentLocalRotationQuaternion ORot = Client.GetSegmentRotation(SubjectName, BoneName );
      if (ORot.Result == Result.Success)
      {
        // mapping back to default data stream axis
        //Quaternion Rot = new Quaternion(-(float)ORot.Rotation[2], -(float)ORot.Rotation[0], (float)ORot.Rotation[1], (float)ORot.Rotation[3]);
        Quaternion Rot = new Quaternion((float)ORot.Rotation[0], (float)ORot.Rotation[1], (float)ORot.Rotation[2], (float)ORot.Rotation[3]);
                // mapping right hand to left hand flipping x

                //_filteredRot.Value = new Vector4(Rot.x, Rot.y, Rot.z, Rot.w);
                //_adaptiveRot.Value = new Vector4(Rot.x, Rot.y, Rot.z, Rot.w);
                Bone.localRotation = new Quaternion(-Rot.y, -Rot.z, Rot.x, Rot.w);
                //Bone.localRotation = new Quaternion(-_filteredRot.Value.y, -_filteredRot.Value.z, _filteredRot.Value.x, _filteredRot.Value.w);
                //Bone.localRotation = new Quaternion(-_adaptiveRot.Value.y, -_adaptiveRot.Value.z, _adaptiveRot.Value.x, _adaptiveRot.Value.w);
            }

      Output_GetSegmentLocalTranslation OTran;
      if (IsScaled)
      {
        OTran = Client.GetScaledSegmentTranslation(SubjectName, BoneName);
      }
      else
      {
        OTran = Client.GetSegmentTranslation(SubjectName, BoneName);
      }

      if (OTran.Result == Result.Success)
      {
        //Vector3 Translate = new Vector3(-(float)OTran.Translation[2] * 0.001f, -(float)OTran.Translation[0] * 0.001f, (float)OTran.Translation[1] * 0.001f);
        Vector3 Translate = new Vector3((float)OTran.Translation[0] * 0.001f, (float)OTran.Translation[1] * 0.001f, (float)OTran.Translation[2] * 0.001f);
        Bone.localPosition = new Vector3(Translate.y, Translate.z, -Translate.x);
        //_filteredPos.Value = Translate;
        //_adaptivePos.Value = Translate;
        
       
        //Bone.localPosition = new Vector3(_filteredPos.Value.y, _filteredPos.Value.z, -_filteredPos.Value.x);
        //Bone.localPosition = new Vector3(_adaptivePos.Value.y, _adaptivePos.Value.z, -_adaptivePos.Value.x);
            }

      // If there's a scale for this subject in the datastream, apply it here.
      if (IsScaled)
      {
        Output_GetSegmentStaticScale OScale = Client.GetSegmentScale(SubjectName, BoneName);
        if (OScale.Result == Result.Success)
        {
          Bone.localScale = new Vector3((float)OScale.Scale[0], (float)OScale.Scale[1], (float)OScale.Scale[2]);
        }
      }
    }
  } //end of program
}// end of namespace

