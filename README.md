# CrowdSimulationReader

## Unity 2019.3.15f1

## 사용법(Usage)
Crowd Simulator에서 뽑은 데이터를 이용하여 Unity에서 포지션에 관한 파일을 읽은 후 시뮬레이션하는 프로젝트입니다.
모델을 읽어들이는 소프트웨어를 개발하지 않고 position값을 뽑아낸 후 읽어서 Unity에서 시뮬레이션합니다.
해당 프로젝트 안에는 모델 파일이 없으므로 기본으로 Cube를 이용하여서 시뮬레이션 할 수 있습니다.
사람 모델을 이용하고 싶다면 모델 파일을 import 한 후 position을 (0,0,0)로 초기화 합니다.
그 후 Prefab으로 만들고 해당 Prefab을 Manager 객체의 Manager.cs의 PeopleObj 에 넣습니다.

It is a project that reads and simulates files in Unity using position data extracted from Crowd Simulatior.
Instead of developing software to read models, the position values are extracted, read and simulate them in Unity.
There is no model file in the project, so you can basically simulate it using Cube.
If you want to use a human model, import the model file and reset the position to (0,0,0).
Then make it Prefab and put it in PeopleObj of the Manager object at Manager.cs.
