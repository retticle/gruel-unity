public interface ITimeDilatable {

	void SetCustomTimeDilation(float timeDilation);
	void AddTimeDilationAffector(UnityEngine.Object obj, float timeDilation);
	void RemoveTimeDilationAffector(UnityEngine.Object obj);

}
