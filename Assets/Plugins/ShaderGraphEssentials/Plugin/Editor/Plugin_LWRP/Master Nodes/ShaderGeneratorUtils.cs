//
// ShaderGraphEssentials for Unity
// (c) 2019 PH Graphics
// Source code may be used and modified for personal or commercial projects.
// Source code may NOT be redistributed or sold.
// 
// *** A NOTE ABOUT PIRACY ***
// 
// If you got this asset from a pirate site, please consider buying it from the Unity asset store. This asset is only legally available from the Unity Asset Store.
// 
// I'm a single indie dev supporting my family by spending hundreds and thousands of hours on this and other assets. It's very offensive, rude and just plain evil to steal when I (and many others) put so much hard work into the software.
// 
// Thank you.
//
// *** END NOTE ABOUT PIRACY ***
//

using System.Linq;
using UnityEditor.ShaderGraph;

namespace ShaderGraphEssentials
{
    internal static class ShaderGeneratorUtils
    {
         public static void GenerateStandardTransforms(int interpolatorStartIndex,
             int maxInterpolators,
             ShaderStringBuilder vertexOutputStruct,
             ShaderStringBuilder vertexShader,
             ShaderStringBuilder vertexShaderDescriptionInputs,
             ShaderStringBuilder vertexShaderTransforms,
             ShaderStringBuilder vertexShaderOutputs,
             ShaderStringBuilder pixelShader,
             ShaderStringBuilder pixelShaderSurfaceInputs,
             ShaderGraphRequirements pixelRequirements,
             ShaderGraphRequirements surfaceRequirements,
             ShaderGraphRequirements modelRequiements,
             ShaderGraphRequirements vertexRequirements,
             CoordinateSpace preferredCoordinateSpace, 
             bool recomputePositionAfterVertexTransform)
        {
            // ----------------------------------------------------- //
            //                         SETUP                         //
            // ----------------------------------------------------- //

            // -------------------------------------
            // Tangent space requires going via World space

            if (preferredCoordinateSpace == CoordinateSpace.Tangent)
                preferredCoordinateSpace = CoordinateSpace.World;

            // -------------------------------------
            // Generate combined graph requirements

            var graphModelRequirements = pixelRequirements.Union(modelRequiements);
            var combinedRequirements = vertexRequirements.Union(graphModelRequirements);
            int interpolatorIndex = interpolatorStartIndex;

            // ----------------------------------------------------- //
            //           GENERATE VERTEX > PIXEL PIPELINE            //
            // ----------------------------------------------------- //

            // -------------------------------------
            // If any stage requires component write into vertex stage
            // If model or pixel requires component write into interpolators and pixel stage

            // -------------------------------------
            // Position

            if (combinedRequirements.requiresPosition > 0)
            {
                var name = preferredCoordinateSpace.ToVariableName(InterpolatorType.Position);
                var preferredSpacePosition = ShaderGenerator.ConvertBetweenSpace("v.vertex", CoordinateSpace.Object, preferredCoordinateSpace, ShaderGenerator.InputType.Position);
                vertexShader.AppendLine("float3 {0} = {1}.xyz;", name, preferredSpacePosition);
                if (graphModelRequirements.requiresPosition > 0)
                {
                    if (recomputePositionAfterVertexTransform)
                        vertexShaderTransforms.AppendLine("{0} = {1}.xyz;", name, preferredSpacePosition);
                    vertexOutputStruct.AppendLine("float3 {0} : TEXCOORD{1};", name, interpolatorIndex);
                    vertexShaderOutputs.AppendLine("o.{0} = {0};", name);
                    pixelShader.AppendLine("float3 {0} = IN.{0};", name);
                    interpolatorIndex++;
                }
            }

            // -------------------------------------
            // Normal
            // Bitangent needs Normal for x product

            if (combinedRequirements.requiresNormal > 0 || combinedRequirements.requiresBitangent > 0)
            {
                var name = preferredCoordinateSpace.ToVariableName(InterpolatorType.Normal);
                vertexShader.AppendLine("float3 {0} = normalize({1});", name, ShaderGenerator.ConvertBetweenSpace("v.normal", CoordinateSpace.Object, preferredCoordinateSpace, ShaderGenerator.InputType.Normal));
                if (graphModelRequirements.requiresNormal > 0 || graphModelRequirements.requiresBitangent > 0)
                {
                    vertexOutputStruct.AppendLine("float3 {0} : TEXCOORD{1};", name, interpolatorIndex);
                    vertexShaderOutputs.AppendLine("o.{0} = {0};", name);
                    pixelShader.AppendLine("float3 {0} = IN.{0};", name);
                    interpolatorIndex++;
                }
            }

            // -------------------------------------
            // Tangent

            if (combinedRequirements.requiresTangent > 0 || combinedRequirements.requiresBitangent > 0)
            {
                var name = preferredCoordinateSpace.ToVariableName(InterpolatorType.Tangent);
                vertexShader.AppendLine("float3 {0} = normalize({1});", name, ShaderGenerator.ConvertBetweenSpace("v.tangent.xyz", CoordinateSpace.Object, preferredCoordinateSpace, ShaderGenerator.InputType.Vector));
                if (graphModelRequirements.requiresTangent > 0 || graphModelRequirements.requiresBitangent > 0)
                {
                    vertexOutputStruct.AppendLine("float3 {0} : TEXCOORD{1};", name, interpolatorIndex);
                    vertexShaderOutputs.AppendLine("o.{0} = {0};", name);
                    pixelShader.AppendLine("float3 {0} = IN.{0};", name);
                    interpolatorIndex++;
                }
            }

            // -------------------------------------
            // Bitangent

            if (combinedRequirements.requiresBitangent > 0)
            {
                var name = preferredCoordinateSpace.ToVariableName(InterpolatorType.BiTangent);
                vertexShader.AppendLine("float3 {0} = cross({1}, {2}.xyz) * {3};",
                    name,
                    preferredCoordinateSpace.ToVariableName(InterpolatorType.Normal),
                    preferredCoordinateSpace.ToVariableName(InterpolatorType.Tangent),
                    "v.tangent.w");
                if (graphModelRequirements.requiresBitangent > 0)
                {
                    vertexOutputStruct.AppendLine("float3 {0} : TEXCOORD{1};", name, interpolatorIndex);
                    vertexShaderOutputs.AppendLine("o.{0} = {0};", name);
                    pixelShader.AppendLine("float3 {0} = IN.{0};", name);
                    interpolatorIndex++;
                }
            }

            // -------------------------------------
            // View Direction

            if (combinedRequirements.requiresViewDir > 0)
            {
                var name = preferredCoordinateSpace.ToVariableName(InterpolatorType.ViewDirection);
                const string worldSpaceViewDir = "_WorldSpaceCameraPos.xyz - mul(GetObjectToWorldMatrix(), float4(v.vertex.xyz, 1.0)).xyz";
                var preferredSpaceViewDir = ShaderGenerator.ConvertBetweenSpace(worldSpaceViewDir, CoordinateSpace.World, preferredCoordinateSpace, ShaderGenerator.InputType.Vector);
                vertexShader.AppendLine("float3 {0} = {1};", name, preferredSpaceViewDir);
                if (graphModelRequirements.requiresViewDir > 0)
                {
                    if (recomputePositionAfterVertexTransform)
                        vertexShaderTransforms.AppendLine("{0} = {1};", name, preferredSpaceViewDir);
                    vertexOutputStruct.AppendLine("float3 {0} : TEXCOORD{1};", name, interpolatorIndex);
                    vertexShaderOutputs.AppendLine("o.{0} = {0};", name);
                    pixelShader.AppendLine("float3 {0} = IN.{0};", name);
                    interpolatorIndex++;
                }
            }

            // -------------------------------------
            // Tangent space transform matrix

            if (combinedRequirements.NeedsTangentSpace())
            {
                var tangent = preferredCoordinateSpace.ToVariableName(InterpolatorType.Tangent);
                var bitangent = preferredCoordinateSpace.ToVariableName(InterpolatorType.BiTangent);
                var normal = preferredCoordinateSpace.ToVariableName(InterpolatorType.Normal);
                if (vertexRequirements.NeedsTangentSpace())
                    vertexShader.AppendLine("float3x3 tangentSpaceTransform = float3x3({0},{1},{2});",
                        tangent, bitangent, normal);
                if (graphModelRequirements.NeedsTangentSpace())
                    pixelShader.AppendLine("float3x3 tangentSpaceTransform = float3x3({0},{1},{2});",
                        tangent, bitangent, normal);
            }

            // -------------------------------------
            // Vertex Color

            if (combinedRequirements.requiresVertexColor)
            {
                var vertexColor = "v.color";
                vertexShader.AppendLine("float4 {0} = {1};", ShaderGeneratorNames.VertexColor, vertexColor);
                if (graphModelRequirements.requiresVertexColor)
                {
                    vertexOutputStruct.AppendLine("float4 {0} : COLOR;", ShaderGeneratorNames.VertexColor);
                    vertexShaderOutputs.AppendLine("o.{0} = {0};", ShaderGeneratorNames.VertexColor);
                    pixelShader.AppendLine("float4 {0} = IN.{0};", ShaderGeneratorNames.VertexColor);
                }
            }

            // -------------------------------------
            // Screen Position

            if (combinedRequirements.requiresScreenPosition)
            {
                var screenPosition = "ComputeScreenPos(mul(GetWorldToHClipMatrix(), mul(GetObjectToWorldMatrix(), v.vertex)), _ProjectionParams.x)";
                vertexShader.AppendLine("float4 {0} = {1};", ShaderGeneratorNames.ScreenPosition, screenPosition);
                if (graphModelRequirements.requiresScreenPosition)
                {
                    if (recomputePositionAfterVertexTransform)
                        vertexShaderTransforms.AppendLine("{0} = {1};", ShaderGeneratorNames.ScreenPosition, screenPosition);
                    vertexOutputStruct.AppendLine("float4 {0} : TEXCOORD{1};", ShaderGeneratorNames.ScreenPosition, interpolatorIndex);
                    vertexShaderOutputs.AppendLine("o.{0} = {0};", ShaderGeneratorNames.ScreenPosition);
                    pixelShader.AppendLine("float4 {0} = IN.{0};", ShaderGeneratorNames.ScreenPosition);
                    interpolatorIndex++;
                }
            }

            // -------------------------------------
            // UV

            foreach (var channel in combinedRequirements.requiresMeshUVs.Distinct())
                vertexShader.AppendLine("float4 {0} = v.texcoord{1};", channel.GetUVName(), (int)channel);
            foreach (var channel in graphModelRequirements.requiresMeshUVs.Distinct())
            {
                vertexOutputStruct.AppendLine("half4 {0} : TEXCOORD{1};", channel.GetUVName(), interpolatorIndex == 0 ? "" : interpolatorIndex.ToString());
                vertexShaderOutputs.AppendLine("o.{0} = {0};", channel.GetUVName());
                pixelShader.AppendLine("float4 {0} = IN.{0};", channel.GetUVName());
                interpolatorIndex++;
            }

            // ----------------------------------------------------- //
            //                TRANSLATE NEEDED SPACES                //
            // ----------------------------------------------------- //

            // -------------------------------------
            // Generate the space translations needed by the vertex description

            ShaderGenerator.GenerateSpaceTranslations(vertexRequirements.requiresNormal, InterpolatorType.Normal, preferredCoordinateSpace,
                ShaderGenerator.InputType.Normal, vertexShader, ShaderGenerator.Dimension.Three);
            ShaderGenerator.GenerateSpaceTranslations(vertexRequirements.requiresTangent, InterpolatorType.Tangent, preferredCoordinateSpace,
                ShaderGenerator.InputType.Vector, vertexShader, ShaderGenerator.Dimension.Three);
            ShaderGenerator.GenerateSpaceTranslations(vertexRequirements.requiresBitangent, InterpolatorType.BiTangent, preferredCoordinateSpace,
                ShaderGenerator.InputType.Vector, vertexShader, ShaderGenerator.Dimension.Three);
            ShaderGenerator.GenerateSpaceTranslations(vertexRequirements.requiresViewDir, InterpolatorType.ViewDirection, preferredCoordinateSpace,
                ShaderGenerator.InputType.Vector, vertexShader, ShaderGenerator.Dimension.Three);
            ShaderGenerator.GenerateSpaceTranslations(vertexRequirements.requiresPosition, InterpolatorType.Position, preferredCoordinateSpace,
                ShaderGenerator.InputType.Position, vertexShader, ShaderGenerator.Dimension.Three);

            // -------------------------------------
            // Generate the space translations needed by the surface description and model

            ShaderGenerator.GenerateSpaceTranslations(graphModelRequirements.requiresNormal, InterpolatorType.Normal, preferredCoordinateSpace,
                ShaderGenerator.InputType.Normal, pixelShader, ShaderGenerator.Dimension.Three);
            ShaderGenerator.GenerateSpaceTranslations(graphModelRequirements.requiresTangent, InterpolatorType.Tangent, preferredCoordinateSpace,
                ShaderGenerator.InputType.Vector, pixelShader, ShaderGenerator.Dimension.Three);
            ShaderGenerator.GenerateSpaceTranslations(graphModelRequirements.requiresBitangent, InterpolatorType.BiTangent, preferredCoordinateSpace,
                ShaderGenerator.InputType.Vector, pixelShader, ShaderGenerator.Dimension.Three);
            ShaderGenerator.GenerateSpaceTranslations(graphModelRequirements.requiresViewDir, InterpolatorType.ViewDirection, preferredCoordinateSpace,
                ShaderGenerator.InputType.Vector, pixelShader, ShaderGenerator.Dimension.Three);
            ShaderGenerator.GenerateSpaceTranslations(graphModelRequirements.requiresPosition, InterpolatorType.Position, preferredCoordinateSpace,
                ShaderGenerator.InputType.Position, pixelShader, ShaderGenerator.Dimension.Three);

            // ----------------------------------------------------- //
            //               START SURFACE DESCRIPTION               //
            // ----------------------------------------------------- //

            // -------------------------------------
            // Copy the locally defined values into the surface description
            // structure using the requirements for ONLY the shader graph pixel stage
            // additional requirements have come from the lighting model
            // and are not needed in the shader graph

            {
                var replaceString = "surfaceInput.{0} = {0};";
                ShaderGenerator.GenerateSpaceTranslationSurfaceInputs(surfaceRequirements.requiresNormal, InterpolatorType.Normal, pixelShaderSurfaceInputs, replaceString);
                ShaderGenerator.GenerateSpaceTranslationSurfaceInputs(surfaceRequirements.requiresTangent, InterpolatorType.Tangent, pixelShaderSurfaceInputs, replaceString);
                ShaderGenerator.GenerateSpaceTranslationSurfaceInputs(surfaceRequirements.requiresBitangent, InterpolatorType.BiTangent, pixelShaderSurfaceInputs, replaceString);
                ShaderGenerator.GenerateSpaceTranslationSurfaceInputs(surfaceRequirements.requiresViewDir, InterpolatorType.ViewDirection, pixelShaderSurfaceInputs, replaceString);
                ShaderGenerator.GenerateSpaceTranslationSurfaceInputs(surfaceRequirements.requiresPosition, InterpolatorType.Position, pixelShaderSurfaceInputs, replaceString);
            }

            if (pixelRequirements.requiresVertexColor)
                pixelShaderSurfaceInputs.AppendLine("surfaceInput.{0} = {0};", ShaderGeneratorNames.VertexColor);

            if (pixelRequirements.requiresScreenPosition)
                pixelShaderSurfaceInputs.AppendLine("surfaceInput.{0} = {0};", ShaderGeneratorNames.ScreenPosition);

            if (pixelRequirements.requiresFaceSign)
                pixelShaderSurfaceInputs.AppendLine("surfaceInput.{0} = {0};", ShaderGeneratorNames.FaceSign);

            foreach (var channel in pixelRequirements.requiresMeshUVs.Distinct())
                pixelShaderSurfaceInputs.AppendLine("surfaceInput.{0} = {0};", ShaderGeneratorNames.GetUVName(channel));
            
            if (pixelRequirements.requiresTime)
            {
                pixelShaderSurfaceInputs.AppendLine("surfaceInput.{0} = _TimeParameters.xyz;", ShaderGeneratorNames.TimeParameters);
            }

            // ----------------------------------------------------- //
            //                START VERTEX DESCRIPTION               //
            // ----------------------------------------------------- //

            // -------------------------------------
            // Copy the locally defined values into the vertex description
            // structure using the requirements for ONLY the shader graph vertex stage
            // additional requirements have come from the lighting model
            // and are not needed in the shader graph

            {
                var replaceString = "vdi.{0} = {0};";
                ShaderGenerator.GenerateSpaceTranslationSurfaceInputs(vertexRequirements.requiresNormal, InterpolatorType.Normal, vertexShaderDescriptionInputs, replaceString);
                ShaderGenerator.GenerateSpaceTranslationSurfaceInputs(vertexRequirements.requiresTangent, InterpolatorType.Tangent, vertexShaderDescriptionInputs, replaceString);
                ShaderGenerator.GenerateSpaceTranslationSurfaceInputs(vertexRequirements.requiresBitangent, InterpolatorType.BiTangent, vertexShaderDescriptionInputs, replaceString);
                ShaderGenerator.GenerateSpaceTranslationSurfaceInputs(vertexRequirements.requiresViewDir, InterpolatorType.ViewDirection, vertexShaderDescriptionInputs, replaceString);
                ShaderGenerator.GenerateSpaceTranslationSurfaceInputs(vertexRequirements.requiresPosition, InterpolatorType.Position, vertexShaderDescriptionInputs, replaceString);
            }

            if (vertexRequirements.requiresVertexColor)
                vertexShaderDescriptionInputs.AppendLine("vdi.{0} = {0};", ShaderGeneratorNames.VertexColor);

            if (vertexRequirements.requiresScreenPosition)
                vertexShaderDescriptionInputs.AppendLine("vdi.{0} = {0};", ShaderGeneratorNames.ScreenPosition);

            foreach (var channel in vertexRequirements.requiresMeshUVs.Distinct())
                vertexShaderDescriptionInputs.AppendLine("vdi.{0} = {0};", channel.GetUVName(), (int)channel);
            
            if (vertexRequirements.requiresTime)
            {
                vertexShaderDescriptionInputs.AppendLine("vdi.{0} = _TimeParameters.xyz;", ShaderGeneratorNames.TimeParameters);
            }
        }
    }
}