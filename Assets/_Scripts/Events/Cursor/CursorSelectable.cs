using System.Linq;
using CustomInspector;
using UnityEngine;

public class CursorSelectable : MonoBehaviour
{
    public CursorType type;
    public Renderer rd;               //Renderer - MeshRenderer
    public Renderer[] rds;
    [Tooltip("Outline Material")]
    public Material selectableMaterial;

    [Tooltip("Outline Thickness")]
    public float selectableThickness = 0.05f;

    [ReadOnly] public bool on;
    

    //메쉬 합성하기. => 제거 예정.
    #region MeshCombine
    // public static void CombineFast(SkinnedMeshRenderer skinnedMeshRenderer, Transform baseBone, Transform[] bones, Mesh[] meshes, Material material)
    // {
    //     if (meshes.Length == 0)
    //         return;
    
    //     CombineInstance[] combineInstances = new CombineInstance[meshes.Length];
    
    //     for(int i =0;i<meshes.Length;i++)
    //     {
    //         if (meshes[i] == null)
    //             continue;
    
    //         combineInstances[i] = new CombineInstance();
    //         combineInstances[i].transform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(Vector3.zero),new Vector3(100,100,100));
    //         combineInstances[i].mesh = meshes[i];
    //     }
    
    //     //Copy bind poses from first mesh in array
    //     Matrix4x4[] bindPoses = meshes[0].bindposes;
    
    //     Mesh combined_new_mesh = new Mesh();
    //     combined_new_mesh.CombineMeshes(combineInstances,true,false);
    //     combined_new_mesh.bindposes = bindPoses;
    
    //     //Note: Mesh.boneWeights returns a copy of bone weights (this is undocumented)
    //     BoneWeight[] newboneweights = combined_new_mesh.boneWeights;
    
    //     //Realign boneweights
    //     int offset = 0;
    //     for (int i=0;i<meshes.Length;i++)
    //     {
    //         for(int k=0;k<meshes[i].vertexCount;k++)
    //         {
    //             newboneweights[offset + k].boneIndex0 -= bones.Length * i;
    //             newboneweights[offset + k].boneIndex1 -= bones.Length * i;
    //             newboneweights[offset + k].boneIndex2 -= bones.Length * i;
    //             newboneweights[offset + k].boneIndex3 -= bones.Length * i;
    //         }
    
    //         offset += meshes[i].vertexCount;
    //     }
    
    //     combined_new_mesh.boneWeights = newboneweights;
    
    //     combined_new_mesh.RecalculateBounds();
    
    //     skinnedMeshRenderer.sharedMesh = combined_new_mesh;
    //     skinnedMeshRenderer.sharedMaterial = material;
    //     skinnedMeshRenderer.bones = bones;
    //     skinnedMeshRenderer.rootBone = baseBone;
    // }
    #endregion
    
    void Update()
    {
        
    }

    public void SetupRenderer(){
        // if(rds.Length <=0 ) return;

        // [magnitude 로 최대 크기의 메쉬 구하기 실패...]
        // rds = GetComponentsInChildren<SkinnedMeshRenderer>();
        // if(rds == null){
        //     rds = GetComponentsInChildren<MeshRenderer>();
        // }

        // Debug.Log($"Renderer Size : {rds.Length}");
        
        // //이제 이 renderer에서 가장 큰 사이즈의 Renderer를 선택한다.
        // rd = rds[0];
        // float maxMag = rds[0].bounds.extents.magnitude;

        // for(int i=0;i<rds.Length;i++){
        //     if(rds[i].bounds.extents.magnitude > rd.bounds.extents.magnitude){
        //         maxMag = rds[i].bounds.extents.magnitude;
        //         rd = rds[i];
        //     }
        // }

        if(rd != null) return;

        rd = GetComponentInChildren<SkinnedMeshRenderer>();

        if(rd == null){
            rd = GetComponentInChildren<MeshRenderer>();
        }
    }

    public void Select(bool on)
    {
        if(rd == null) return;
        
        string layername = on ? "Outline" : "Default";
        
        //Debugging
        if(on){
            Debug.Log($"{gameObject.name}");
        }
        
        this.on = on;
        rd.gameObject.layer = LayerMask.NameToLayer(layername);

        if(selectableMaterial != null)
            selectableMaterial.SetFloat("_Thickness", selectableThickness);
    }
}