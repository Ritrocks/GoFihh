using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurRendererFeature : ScriptableRendererFeature
{
    [SerializeField] Material blurMaterial;
    [SerializeField, Range(0.1f, 10f)] float blurRadius = 1f;

    BlurPass blurPass;

    public override void Create()
    {
        if (blurMaterial != null)
            blurPass = new BlurPass(blurMaterial, blurRadius)
            {
                renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing
            };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (blurPass != null && renderingData.cameraData.cameraType == CameraType.Game)
        {
            blurPass.Setup(renderer.cameraColorTargetHandle);
            renderer.EnqueuePass(blurPass);
        }
    }

    protected override void Dispose(bool disposing)
    {
        blurPass?.Dispose();
    }

    sealed class BlurPass : ScriptableRenderPass
    {
        readonly Material material;
        readonly float radius;
        RTHandle source;
        RTHandle temporaryTexture;

        public BlurPass(Material material, float radius)
        {
            this.material = material;
            this.radius = radius;
        }

        public void Setup(RTHandle source)
        {
            this.source = source;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = 0;
            RenderingUtils.ReAllocateIfNeeded(ref temporaryTexture, descriptor, FilterMode.Bilinear, name: "_BlurTemporaryTexture");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (source == null)
                return;

            material.SetVector("_BlurParams", new Vector4(radius, 0f, 1f, 1f));
            CommandBuffer commandBuffer = CommandBufferPool.Get("Blur Before UI");
            Blitter.BlitCameraTexture(commandBuffer, source, temporaryTexture, material, 1);
            Blitter.BlitCameraTexture(commandBuffer, temporaryTexture, source, material, 2);
            context.ExecuteCommandBuffer(commandBuffer);
            CommandBufferPool.Release(commandBuffer);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            source = null;
        }

        public void Dispose()
        {
            temporaryTexture?.Release();
        }
    }
}