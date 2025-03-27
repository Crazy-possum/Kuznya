using System.Collections.Generic;
using UnityEngine;

public class ForgingStepsView : MonoBehaviour
{
    [SerializeField] private List<WorkZoneUIView> _step1WorkZones;
    [SerializeField] private List<WorkZoneUIView> _step2WorkZones;
    [SerializeField] private List<WorkZoneUIView> _step3WorkZones;
    [SerializeField] private List<WorkZoneUIView> _step4WorkZones;
    
    public List<WorkZoneUIView> Step1WorkZones => _step1WorkZones;
    public List<WorkZoneUIView> Step2WorkZones => _step2WorkZones;
    public List<WorkZoneUIView> Step3WorkZones => _step3WorkZones;
    public List<WorkZoneUIView> Step4WorkZones => _step4WorkZones;
}
