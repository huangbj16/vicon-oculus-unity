# vicon_test UNITY project

This folder is a unity project that uses Vicon MoCap system and Oculus Quest2 headset to render VR apps with haptic feedback. This readme explains the strucutre of the code.

- TCPClient.cs: send command to bluetooth haptic device. 

- StylusTrigger.cs: use stylus object as the main controller to trigger vfx and haptic effects

- VisualEffects folder: contains self-designed vfx effects that render under universal rendering pipeline (URP)

- ViconDataStreamPrefab: the main object that receives data stream from vicon. provided by Vicon SDK.

- HMDPrefab: contains the PoseProvider and HMDScript. HMDScript receives hmd 3D position data from DataStreamPrefab and converts them into headset position in Unity.

- TrackingObject folder: contains objects that are tracked by vicon camera. Each object contains the RBScript (Rigid Body). Similarly, it receives object 3D position from DataStreamPrefab and converts them into object position in Unity. 
    
    one special object is the group of actuators, which is the virtual representation of the physical actuators. It is also tied to the vfx effects.

    another speical object is the stylus, which is the main object that triggers the collision with other objects (e.g., actuators)

- DataCollection.cs: the file that records experimetn data and saves to file.