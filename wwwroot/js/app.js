const scene = new THREE.Scene();
scene.background = new THREE.Color(0x222222);

const camera = new THREE.PerspectiveCamera(45, window.innerWidth / window.innerHeight, 0.1, 1000);

const renderer = new THREE.WebGLRenderer({ antialias: true });
renderer.setSize(window.innerWidth, window.innerHeight);
renderer.setPixelRatio(window.devicePixelRatio);
document.body.appendChild(renderer.domElement);

const controls = new THREE.OrbitControls(camera, renderer.domElement);
controls.enableDamping = true;
controls.dampingFactor = 0.05;

const ambientLight = new THREE.AmbientLight(0xffffff, 1.2);
scene.add(ambientLight);

const directionalLight = new THREE.DirectionalLight(0xffffff, 2.0);
directionalLight.position.set(10, 20, 10);
scene.add(directionalLight);

const backLight = new THREE.DirectionalLight(0xffffff, 0.8);
backLight.position.set(-10, -20, -10);
scene.add(backLight);

const loader = new THREE.GLTFLoader();
loader.load('/models/part.glb', function (gltf) 
{
    scene.add(gltf.scene);
    const box = new THREE.box3().setFromObject(gltf.scene);
    const center = box.getCenter(new THREE.Vector3());
    const size = box.getSize(new THREE.Vector3());

    controls.target.copy(center);
    camera.position.set(center.x + size.length() * 1.5, center.y + size.length() * 1.5, center.x + size.length() * 1.5);
    controls.update();

    document.getElementById('loading').style.display = 'none';
},
    function (xhr)
    {
        console.log((xhr.loaded / xhr.total * 100) + '% loaded'); 
    },
    function (error)
    {
    console.error('An Error Occured Loading The Model:', error);
    document.getElementById('loading').innerText = 'Error Could Not Load part.glb';
    }
);

window.addEventListener('resize', onWindowResize, false);
function onWindowResize()
{
    camera.aspect = window.innerWidth / window.innerHeight;
    camera.updateProjectMatrix();
    renderer.setSize(window.innerWidth, window.innerHeight);
}
function animate()
{
    requestAnimationFrame(animate);
    controls.update();
    renderer.render(scene, camera);
}
animate();

