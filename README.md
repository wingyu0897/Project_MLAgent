<h1>아이 드리프트 (Ai Drift)</h1>

<h3>게임 소개</h3>
아이 드리프트는 AI 차량과 대전하는 1v1 캐주얼 레이싱 게임입니다.<br>
ML-Agent 프로젝트로 제작하였으며, ML-Agent를 사용하여 AI를 학습하였습니다.<br><br>

<img src="https://i.imgur.com/9dvS8sr.png" width="700">

---

<h3>주요 기능 설명</h3>
<b>GameSceneManager : </b> 게임씬의 준비, 진행, 종료 등의 상태를 관리하는 스크립트입니다. 싱글톤으로 제작되었으며 옵저버 패턴을 사용해 상태가 병경되었음을 알립니다.<br>
<b>CarController : </b> 차량의 움직임을 제어하는 스크립트입니다. 정해진 수식을 통해 가속, 감속, 마찰, 충돌 등을 제어하고 관리합니다.<br>
<b>CarInputAI : </b> Agent를 상속 받은 클래스로서 AI 학습과 학습된 AI를 제어합니다.<br>
<b>UIComponents : </b> 리스트를 통해 모든 UI 컴포넌트를 저장하는 스크립트입니다.<br>
<b>CarSelect</b> : Scrollbar UI와 간단한 수식을 사용해 차량을 선택할 수 있도록하는 스크립트입니다.<br>

---

### [[완성본 바로가기]](https://drive.google.com/file/d/13Mp2w11gBhvqJDGx_0nq-PvtHmXFdW1n/view?usp=sharing)
