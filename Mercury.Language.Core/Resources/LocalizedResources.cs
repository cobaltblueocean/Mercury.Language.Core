// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
//
// Please see distribution for license.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
//     
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Mercury.Language.Core.Properties;

namespace Mercury.Language.Core
{
    //using Properties = Mercury.Language.Core.Properties;

    /// <summary>
    /// LocalizedResources Description
    /// </summary>
    public class LocalizedResources : INotifyPropertyChanged
    {
        #region Singleton Class Implementation
        private readonly Resources resources = new Resources();

        private static LocalizedResources instance;

        /// <summary>
        /// Constructor implement as Singleton pattern
        /// </summary>
        private LocalizedResources()
        {
        }

        /// <summary>
        /// Return singleton instance
        /// </summary>
        /// <returns>Return current instance</returns>
        public static LocalizedResources Instance()
        {
            if (instance == null)
                instance = new LocalizedResources();

            return instance;
        }

        /// <summary>
        /// Hangling culture changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Change resource culture change
        /// </summary>
        /// <param name="name"></param>
        public void ChangeCulture(string name)
        {
            Resources.Culture = CultureInfo.GetCultureInfo(name);
            RaisePropertyChanged("Resources");
        }

        /// <summary>
        /// Get resource
        /// </summary>
        internal Resources Resources
        {
            get { return resources; }
        }

        #endregion

        public String CannotCompareValues
        {
            get { return Properties.Resources.CannotCompareValues; }
        }

        public String CannotCompareProperty
        {
            get { return Properties.Resources.CannotCompareProperty; }
        }

        public String MismatchWithPropertyFound
        {
            get { return Properties.Resources.MismatchWithPropertyFound; }
        }

        public String ItemInPropertyCollectionDoesNotMatch
        {
            get { return Properties.Resources.ItemInPropertyCollectionDoesNotMatch; }
        }

        public String CollectionCountsForPropertyDoNotMatch
        {
            get { return Properties.Resources.CollectionCountsForPropertyDoNotMatch; }
        }
        public String CoreSourceDestinationObjectsAreNull
        {
            get { return Resources.CoreSourceDestinationObjectsAreNull; }
        }

        public String Exception_FromToSize
        {
            get { return Resources.Exception_FromToSize; }
        }

        public String PropertyOrMethodNotImplemented
        {
            get { return Resources.PropertyOrMethodNotImplemented; }
        }

        public String BIGDECIMAL_THE_VALUE_CANNOT_FIT_INTO
        {
            get { return Resources.BIGDECIMAL_THE_VALUE_CANNOT_FIT_INTO; }
        }

        public String BIGDECIMAL_CANNOT_CAST_TO_CHAR
        {
            get { return Resources.BIGDECIMAL_CANNOT_CAST_TO_CHAR; }
        }

        public String BIGDECIMAL_CANNOT_CAST_TO_DATETIME
        {
            get { return Resources.BIGDECIMAL_CANNOT_CAST_TO_DATETIME; }
        }

        public String BIGDECIMAL_COMPARE_TO_OBJECT_MUST_BE_A_BIGDECIMAL
        {
            get { return Resources.BIGDECIMAL_COMPARE_TO_OBJECT_MUST_BE_A_BIGDECIMAL; }
        }

        public String BIGINTEGER_VALUE
        {
            get { return Resources.BIGINTEGER_VALUE; }
        }
        public String BITARRAY_FROMINDEX_IS_NEGATIVE
        {
            get { return Resources.BITARRAY_FROMINDEX_IS_NEGATIVE; }
        }

        public String AutoParallel_ThresholdValueNegative
        {
            get { return Resources.AutoParallel_ThresholdValueNegative; }
        }
        public String OVERFLOW
        {
            get { return Resources.OVERFLOW; }
        }
        public String OVERFLOW_IN_FRACTION
        {
            get { return Resources.OVERFLOW_IN_FRACTION; }
        }
        public String OVERFLOW_IN_ADDITION
        {
            get { return Resources.OVERFLOW_IN_ADDITION; }
        }
        public String OVERFLOW_IN_SUBTRACTION
        {
            get { return Resources.OVERFLOW_IN_SUBTRACTION; }
        }
        public String OVERFLOW_IN_MULTIPLICATION
        {
            get { return Resources.OVERFLOW_IN_MULTIPLICATION; }
        }
        public String OVERFLOW_TIMESPAN_TOO_LONG
        {
            get { return Resources.OVERFLOW_TIMESPAN_TOO_LONG; }
        }
        public String OVERFLOW_DURATION
        {
            get { return Resources.OVERFLOW_DURATION; }
        }
        public String OVERFLOW_NEGATE_TWOS_COMP_NUM
        {
            get { return Resources.OVERFLOW_NEGATE_TWOS_COMP_NUM; }
        }

        public String GCD_OVERFLOW_32_BITS
        {
            get { return Resources.GCD_OVERFLOW_32_BITS; }
        }
        public String GCD_OVERFLOW_64_BITS
        {
            get { return Resources.GCD_OVERFLOW_64_BITS; }
        }

        public String LCM_OVERFLOW_32_BITS
        {
            get { return Resources.LCM_OVERFLOW_32_BITS; }
        }
        public String LCM_OVERFLOW_64_BITS
        {
            get { return Resources.LCM_OVERFLOW_64_BITS; }
        }
        public String NOT_DECREASING_NUMBER_OF_POINTS
        {
            get { return Resources.NOT_DECREASING_NUMBER_OF_POINTS; }
        }
        public String NOT_DECREASING_SEQUENCE
        {
            get { return Resources.NOT_DECREASING_SEQUENCE; }
        }

        public String NOT_INCREASING_NUMBER_OF_POINTS
        {
            get { return Resources.NOT_INCREASING_NUMBER_OF_POINTS; }
        }
        public String NOT_INCREASING_SEQUENCE
        {
            get { return Resources.NOT_INCREASING_SEQUENCE; }
        }

        public String NOT_STRICTLY_DECREASING_NUMBER_OF_POINTS
        {
            get { return Resources.NOT_STRICTLY_DECREASING_NUMBER_OF_POINTS; }
        }
        public String NOT_STRICTLY_DECREASING_SEQUENCE
        {
            get { return Resources.NOT_STRICTLY_DECREASING_SEQUENCE; }
        }
        public String NOT_STRICTLY_INCREASING_KNOT_VALUES
        {
            get { return Resources.NOT_STRICTLY_INCREASING_KNOT_VALUES; }
        }
        public String NOT_STRICTLY_INCREASING_NUMBER_OF_POINTS
        {
            get { return Resources.NOT_STRICTLY_INCREASING_NUMBER_OF_POINTS; }
        }
        public String NOT_STRICTLY_INCREASING_SEQUENCE
        {
            get { return Resources.NOT_STRICTLY_INCREASING_SEQUENCE; }
        }
        public String METHOD_ADDITION_OVERFLOWS_INT
        {
            get { return Resources.METHOD_ADDITION_OVERFLOWS_INT; }
        }

        public String METHOD_ADDITION_OVERFLOWS_LONG
        {
            get { return Resources.METHOD_ADDITION_OVERFLOWS_LONG; }
        }

        public String METHOD_SUBTRACTION_OVERFLOWS_INT
        {
            get { return Resources.METHOD_SUBTRACTION_OVERFLOWS_INT; }
        }

        public String METHOD_SUBTRACTION_OVERFLOWS_LONG
        {
            get { return Resources.METHOD_SUBTRACTION_OVERFLOWS_LONG; }
        }

        public String METHOD_MULTIPLICTION_OVERFLOWS_INT
        {
            get { return Resources.METHOD_MULTIPLICTION_OVERFLOWS_INT; }
        }

        public String METHOD_MULTIPLICTION_OVERFLOWS_LONG
        {
            get { return Resources.METHOD_MULTIPLICTION_OVERFLOWS_LONG; }
        }

        public String METHOD_CALCULACTION_OVERFLOWS_INT
        {
            get { return Resources.METHOD_CALCULACTION_OVERFLOWS_INT; }
        }

        public String METHOD_PARAMETER_MUST_NOT_BE_NULL
        {
            get { return Resources.METHOD_PARAMETER_MUST_NOT_BE_NULL; }
        }
        public String ARRAY_ELEMENT
        {
            get { return Resources.ARRAY_ELEMENT; }
        }
        public String ARRAY_INVALID_ORDER_DIRECTION
        {
            get { return Resources.ARRAY_INVALID_ORDER_DIRECTION; }
        }
        public String ARRAY_THE_ARRAY_IS_NON_MONOTONIC_SEQUENCE
        {
            get { return Resources.ARRAY_THE_ARRAY_IS_NON_MONOTONIC_SEQUENCE; }
        }
        public String ARRAY_ORDER
        {
            get { return Resources.ARRAY_ORDER; }
        }
        public String ARRAY_SIZE_EXCEEDS_MAX_VARIABLES
        {
            get { return Resources.ARRAY_SIZE_EXCEEDS_MAX_VARIABLES; }
        }
        public String ARRAY_SIZES_SHOULD_HAVE_DIFFERENCE_1
        {
            get { return Resources.ARRAY_SIZES_SHOULD_HAVE_DIFFERENCE_1; }
        }
        public String ARRAY_SUMS_TO_ZERO
        {
            get { return Resources.ARRAY_SUMS_TO_ZERO; }
        }
        public String ASSERTION_FAILURE
        {
            get { return Resources.ASSERTION_FAILURE; }
        }

        public String INVALID_OPERATION_ZERO_BASE_AND_NEGATIVE_POWER
        {
            get { return Resources.INVALID_OPERATION_ZERO_BASE_AND_NEGATIVE_POWER; }
        }
        public String INVALID_OPERATION_NEGATIVE_BASE_AND_NON_INTEGER_POWER
        {
            get { return Resources.INVALID_OPERATION_ZERO_BASE_AND_NEGATIVE_POWER; }
        }
        public String LARGEARRAY_SRCPOS_SIZE_ERROR
        {
            get { return Resources.LARGEARRAY_SRCPOS_SIZE_ERROR; }
        }
        public String LARGEARRAY_DESTPOS_SIZE_ERROR
        {
            get { return Resources.LARGEARRAY_DESTPOS_SIZE_ERROR; }
        }
        public String LARGEARRAY_LENGTH_ERROR
        {
            get { return Resources.LARGEARRAY_LENGTH_ERROR; }
        }
        public String LARGEARRAY_CONSTANT_ARRAYS_CANNOT_BE_MODIFIED
        {
            get { return Resources.LARGEARRAY_CONSTANT_ARRAYS_CANNOT_BE_MODIFIED; }
        }
        public String Utility_Extension_Array_SetRow_TheValueArrayMustBeSameLengthOfTheTargetArraysRow
        {
            get { return Resources.Utility_Extension_Array_SetRow_TheValueArrayMustBeSameLengthOfTheTargetArraysRow; }
        }
        public String Utility_Extension_Array_ToUpperTriangular_OnlyLowerTriangularUpperTriangularAndDiagonalMatricesAreSupportedAtThisTime
        {
            get { return Resources.Utility_Extension_Array_ToUpperTriangular_OnlyLowerTriangularUpperTriangularAndDiagonalMatricesAreSupportedAtThisTime; }
        }
        public String Utility_Extension_Array_GetSymmetric_MatrixTypeCanBeEitherLowerTriangularOrUpperTrianguler
        {
            get { return Resources.Utility_Extension_Array_GetSymmetric_MatrixTypeCanBeEitherLowerTriangularOrUpperTrianguler; }
        }
        public String Utility_Extension_Array_Transpose_OnlySquareMatricesCanBeTransposedInPlace
        {
            get { return Resources.Utility_Extension_Array_Transpose_OnlySquareMatricesCanBeTransposedInPlace; }
        }
        public String Utility_Extension_Array_Transpose_TheGivenObjectMustInheritFromSystemArray
        {
            get { return Resources.Utility_Extension_Array_Transpose_TheGivenObjectMustInheritFromSystemArray; }
        }
        public String Utility_ArgumentChecker_String_NotEmpty_InputParameterMustNotBeEmpty
        {
            get { return Resources.Utility_ArgumentChecker_String_NotEmpty_InputParameterMustNotBeEmpty; }
        }
        public String Utility_ArgumentChecker_Array_NotEmpty_InputParameterArrayMustNotBeEmpty
        {
            get { return Resources.Utility_ArgumentChecker_Array_NotEmpty_InputParameterArrayMustNotBeEmpty; }
        }
        public String Utility_ArgumentChecker_Enumerable_NotEmpty_InputParameterIterableMustNotBeEmpty
        {
            get { return Resources.Utility_ArgumentChecker_Enumerable_NotEmpty_InputParameterIterableMustNotBeEmpty; }
        }
        public String Utility_ArgumentChecker_Collection_NotEmpty_InputParameterCollectionMustNotBeEmpty
        {
            get { return Resources.Utility_ArgumentChecker_Collection_NotEmpty_InputParameterCollectionMustNotBeEmpty; }
        }
        public String Utility_ArgumentChecker_Generic_NotNull_InputParameterMustNotBeNull
        {
            get { return Resources.Utility_ArgumentChecker_Generic_NotNull_InputParameterMustNotBeNull; }
        }
        public String Utility_ArgumentChecker_Generic_NotNullInjected_InjectedInputParameterMustNotBeNull
        {
            get { return Resources.Utility_ArgumentChecker_Generic_NotNullInjected_InjectedInputParameterMustNotBeNull; }
        }
        public String Utility_ArgumentChecker_GenericArray_NoNulls_InputParameterArrayMustNotContainNullAtIndex
        {
            get { return Resources.Utility_ArgumentChecker_GenericArray_NoNulls_InputParameterArrayMustNotContainNullAtIndex; }
        }
        public String Utility_ArgumentChecker_GenericEnumerable_NoNulls_InputParameterEnumerableMustNotContainNullAtIndex
        {
            get { return Resources.Utility_ArgumentChecker_GenericEnumerable_NoNulls_InputParameterEnumerableMustNotContainNullAtIndex; }
        }
        public String Utility_ArgumentChecker_GenericArray2D_NoNulls_InputParameter2DArrayMustNotContainNullAtIndex
        {
            get { return Resources.Utility_ArgumentChecker_GenericArray2D_NoNulls_InputParameter2DArrayMustNotContainNullAtIndex; }
        }
        public String Utility_ArgumentChecker_GenericList_NoNulls_InputParameterListMustNotContainNullAtIndex
        {
            get { return Resources.Utility_ArgumentChecker_GenericList_NoNulls_InputParameterListMustNotContainNullAtIndex; }
        }
        public String Utility_ArgumentChecker_DynamicArray_NoNulls_InputParameterArrayMustNotContainNullAtIndex
        {
            get { return Resources.Utility_ArgumentChecker_DynamicArray_NoNulls_InputParameterArrayMustNotContainNullAtIndex; }
        }
        public String Utility_ArgumentChecker_DynamicList_NoNulls_InputParameterListMustNotContainNullAtIndex
        {
            get { return Resources.Utility_ArgumentChecker_DynamicList_NoNulls_InputParameterListMustNotContainNullAtIndex; }
        }
        public String Utility_ArgumentChecker_Long_NotNegativeOrZero_InputParameterMustNotBeNegativeOrZero
        {
            get { return Resources.Utility_ArgumentChecker_Long_NotNegativeOrZero_InputParameterMustNotBeNegativeOrZero; }
        }
        public String Utility_ArgumentChecker_Double_NotNegativeOrZero_InputParameterMustNotBeNegativeOrZero
        {
            get { return Resources.Utility_ArgumentChecker_Double_NotNegativeOrZero_InputParameterMustNotBeNegativeOrZero; }
        }
        public String Utility_ArgumentChecker_Double_AlmostNotZero_InputParameterMustNotBeZero
        {
            get { return Resources.Utility_ArgumentChecker_Double_AlmostNotZero_InputParameterMustNotBeZero; }
        }
        public String Utility_ArgumentChecker_Double_AlmostNotNegative_InputParameterMustBeGreaterThanZero
        {
            get { return Resources.Utility_ArgumentChecker_Double_AlmostNotNegative_InputParameterMustBeGreaterThanZero; }
        }
        public String Utility_ArgumentChecker_Generic_InOrderOrEqual_InputParameterMustBeBefore
        {
            get { return Resources.Utility_ArgumentChecker_Generic_InOrderOrEqual_InputParameterMustBeBefore; }
        }
        public String CANNOT_CALCULATE_SQUARE_ROOT_FROM_A_NEGATIVE_NUMBER
        {
            get { return Resources.CANNOT_CALCULATE_SQUARE_ROOT_FROM_A_NEGATIVE_NUMBER; }
        }
        public String PARAMETER_MUST_BE_IN
        {
            get { return Resources.PARAMETER_MUST_BE_IN; }
        }
        public String PARAMETER_MUST_BE_HIGHER_THAN
        {
            get { return Resources.PARAMETER_MUST_BE_HIGHER_THAN; }
        }

        public String PARAMETER_IS_OUT_OF_RANGE
        {
            get { return Resources.PARAMETER_IS_OUT_OF_RANGE; }
        }
        public String INVALID_ARGUMENTS
        {
            get { return Resources.INVALID_ARGUMENTS; }
        }
        public String RESULT_TOO_LARGE_REPRESENT_IN_A_LONG_INTEGER
        {
            get { return Resources.RESULT_TOO_LARGE_REPRESENT_IN_A_LONG_INTEGER; }
        }

        public String MUST_HAVE_N_IS_MORE_THAN_OR_EQUALS_FOR_N_ABSOLUTE
        {
            get { return Resources.MUST_HAVE_N_IS_MORE_THAN_OR_EQUALS_FOR_N_ABSOLUTE; }
        }

        public String MUST_HAVE_N_IS_MORE_THAN_FOR_N_ABSOLUTE
        {
            get { return Resources.MUST_HAVE_N_IS_MORE_THAN_FOR_N_ABSOLUTE; }
        }
        public String ZERO_DENOMINATOR
        {
            get { return Resources.ZERO_DENOMINATOR; }
        }
        public String ZERO_DENOMINATOR_IN_FRACTION
        {
            get { return Resources.ZERO_DENOMINATOR_IN_FRACTION; }
        }
        public String ZERO_FRACTION_TO_DIVIDE_BY
        {
            get { return Resources.ZERO_FRACTION_TO_DIVIDE_BY; }
        }
        public String ZERO_NOT_ALLOWED
        {
            get { return Resources.ZERO_NOT_ALLOWED; }
        }
        public String SET_IS_A_READ_ONLY
        {
            get { return Resources.SET_IS_A_READ_ONLY; }
        }
        public String DIMENSION
        {
            get { return Resources.DIMENSION; }
        }
        public String DIMENSIONS_MISMATCH_2x2
        {
            get { return Resources.DIMENSIONS_MISMATCH_2x2; }
        }
        public String DIMENSIONS_MISMATCH_SIMPLE
        {
            get { return Resources.DIMENSIONS_MISMATCH_SIMPLE; }
        }
        public String DIMENSIONS_MISMATCH
        {
            get { return Resources.DIMENSIONS_MISMATCH; }
        }
        public String INPUT_ARRAY
        {
            get { return Resources.INPUT_ARRAY; }
        }
        public String LENGTH
        {
            get { return Resources.LENGTH; }
        }
        public String START_POSITION
        {
            get { return Resources.START_POSITION; }
        }
        public String SUBARRAY_ENDS_AFTER_ARRAY_END
        {
            get { return Resources.SUBARRAY_ENDS_AFTER_ARRAY_END; }
        }
        public String ARGUMENT_CANNOT_BE_NEGATIVE
        {
            get { return Resources.ARGUMENT_CANNOT_BE_NEGATIVE; }
        }
        public String ARGUMENT_OUT_OF_RANGE
        {
            get { return Resources.ARGUMENT_OUT_OF_RANGE; }
        }
        public String ARGUMENT_OUTSIDE_DOMAIN
        {
            get { return Resources.ARGUMENT_OUTSIDE_DOMAIN; }
        }
        public String ARGUMENTS_MUST_BE_DIFFERENT_OBJECTS
        {
            get { return Resources.ARGUMENTS_MUST_BE_DIFFERENT_OBJECTS; }
        }
        public String ARITHMETIC_EXCEPTION
        {
            get { return Resources.ARITHMETIC_EXCEPTION; }
        }
        public String SOURCE_AND_INDEXES_ARRAYS_MUST_HAVE_THE_SAME_DIMENSION
        {
            get { return Resources.SOURCE_AND_INDEXES_ARRAYS_MUST_HAVE_THE_SAME_DIMENSION; }
        }
        public String THE_DESTINATION_MATRIX_MUST_BE_BIG_ENOUGH
        {
            get { return Resources.THE_DESTINATION_MATRIX_MUST_BE_BIG_ENOUGH; }
        }
        public String PRINT_ERROR
        {
            get { return Resources.PRINT_ERROR; }
        }

        public String PRINT_FAILED_IN_PRINTING
        {
            get { return Resources.PRINT_FAILED_IN_PRINTING; }
        }
        public String LINKEDDICTIONARY_COULD_NOT_FIND_THE_KEY
        {
            get { return Resources.LINKEDDICTIONARY_COULD_NOT_FIND_THE_KEY; }
        }

        public String LINKEDHASHSET_OTHER_CANNOT_BE_NULL
        {
            get { return Resources.LINKEDHASHSET_OTHER_CANNOT_BE_NULL; }
        }
        public String TREEDICTIONARY_ARRAY_NOT_SUFFICIENT_SIZE
        {
            get { return Resources.TREEDICTIONARY_ARRAY_NOT_SUFFICIENT_SIZE; }
        }

        public String TREEDICTIONARY_AN_ELEMENT_WITH_THE_SAME_KEY_ALREADY_EXISTS
        {
            get { return Resources.TREEDICTIONARY_AN_ELEMENT_WITH_THE_SAME_KEY_ALREADY_EXISTS; }
        }
        public String NUMBER_TOO_LARGE
        {
            get { return Resources.NUMBER_TOO_LARGE; }
        }
        public String NUMBER_TOO_SMALL
        {
            get { return Resources.NUMBER_TOO_SMALL; }
        }
        public String NUMBER_TOO_LARGE_BOUND_EXCLUDED
        {
            get { return Resources.NUMBER_TOO_LARGE_BOUND_EXCLUDED; }
        }
        public String NUMBER_TOO_SMALL_BOUND_EXCLUDED
        {
            get { return Resources.NUMBER_TOO_SMALL_BOUND_EXCLUDED; }
        }
        public String NUMBERS_OF_KEYS_AND_VALUE_NOT_MATCH
        {
            get { return Resources.NUMBERS_OF_KEYS_AND_VALUE_NOT_MATCH; }
        }
        public String OUT_OF_BOUNDS_QUANTILE_VALUE
        {
            get { return Resources.OUT_OF_BOUNDS_QUANTILE_VALUE; }
        }
        public String OUT_OF_BOUNDS_CONFIDENCE_LEVEL
        {
            get { return Resources.OUT_OF_BOUNDS_CONFIDENCE_LEVEL; }
        }
        public String OUT_OF_BOUND_SIGNIFICANCE_LEVEL
        {
            get { return Resources.OUT_OF_BOUND_SIGNIFICANCE_LEVEL; }
        }
        public String OUT_OF_ORDER_ABSCISSA_ARRAY
        {
            get { return Resources.OUT_OF_ORDER_ABSCISSA_ARRAY; }
        }
        public String OUT_OF_RANGE_ROOT_OF_UNITY_INDEX
        {
            get { return Resources.OUT_OF_RANGE_ROOT_OF_UNITY_INDEX; }
        }
        public String OUT_OF_RANGE
        {
            get { return Resources.OUT_OF_RANGE; }
        }
        public String OUT_OF_RANGE_SIMPLE
        {
            get { return Resources.OUT_OF_RANGE_SIMPLE; }
        }
        public String OUT_OF_RANGE_LEFT
        {
            get { return Resources.OUT_OF_RANGE_LEFT; }
        }
        public String OUT_OF_RANGE_RIGHT
        {
            get { return Resources.OUT_OF_RANGE_RIGHT; }
        }
        public String BASE
        {
            get { return Resources.BASE; }
        }
        public String EXPONENT
        {
            get { return Resources.EXPONENT; }
        }
        public String FACTORIAL_NEGATIVE_PARAMETER
        {
            get { return Resources.FACTORIAL_NEGATIVE_PARAMETER; }
        }
        public String METHOD_VALUE_MUST_NOT_BE_NULL
        {
            get { return Resources.METHOD_VALUE_MUST_NOT_BE_NULL; }
        }
        public String MAX_COUNT_EXCEEDED
        {
            get { return Resources.MAX_COUNT_EXCEEDED; }
        }
        public String MAX_ITERATIONS_EXCEEDED
        {
            get { return Resources.MAX_ITERATIONS_EXCEEDED; }
        }
        public String INDEX_LARGER_THAN_MAX
        {
            get { return Resources.INDEX_LARGER_THAN_MAX; }
        }
        public String INDEX_NOT_POSITIVE
        {
            get { return Resources.INDEX_NOT_POSITIVE; }
        }
        public String INDEX_OUT_OF_RANGE
        {
            get { return Resources.INDEX_OUT_OF_RANGE; }
        }
        public String INDEX
        {
            get { return Resources.INDEX; }
        }
        public String INVALID_IMPLEMENTATION
        {
            get { return Resources.INVALID_IMPLEMENTATION; }
        }
        public String n_MUST_BE_GREATER_THAN_ZERO
        {
            get { return Resources.n_MUST_BE_GREATER_THAN_ZERO; }
        }
        public String n_MUST_BE_POSITIVE_INT
        {
            get { return Resources.n_MUST_BE_POSITIVE_INT; }
        }
        public String BINOMIAL_INVALID_PARAMETERS_ORDER
        {
            get { return Resources.BINOMIAL_INVALID_PARAMETERS_ORDER; }
        }
        public String BINOMIAL_NEGATIVE_PARAMETER
        {
            get { return Resources.BINOMIAL_NEGATIVE_PARAMETER; }
        }

        public String RANDOM_DIMENSION_MUST_BE_GREATER_THAN_ZERO
        {
            get { return Resources.RANDOM_DIMENSION_MUST_BE_GREATER_THAN_ZERO; }
        }

        public String RANDOM_NUMBER_OF_VALUES_MUST_BE_GREATER_THAN_ZERO
        {
            get { return Resources.RANDOM_NUMBER_OF_VALUES_MUST_BE_GREATER_THAN_ZERO; }
        }
        public String RANDOMKEY_MUTATION_WRONG_CLASS
        {
            get { return Resources.RANDOMKEY_MUTATION_WRONG_CLASS; }
        }

        public String MATRIX_IS_NOT_POSITIVE_DEFINITE
        {
            get { return Resources.MATRIX_IS_NOT_POSITIVE_DEFINITE; }
        }

        public String MATRIX_CANNOT_BE_NULL
        {
            get { return Resources.MATRIX_CANNOT_BE_NULL; }
        }
        public String MATRIX_IS_NOT_A_SQUARE_MATRIX
        {
            get { return Resources.MATRIX_IS_NOT_A_SQUARE_MATRIX; }
        }
        public String MATRIX_DIMENSIONS_DO_NOT_MATCH
        {
            get { return Resources.MATRIX_DIMENSIONS_DO_NOT_MATCH; }
        }

        public String MATRIX_MUST_BE_SQUARE
        {
            get { return Resources.MATRIX_MUST_BE_SQUARE; }
        }

        public String MATRIX_IS_SINGULAR
        {
            get { return Resources.MATRIX_IS_SINGULAR; }
        }

        public String MATRIX_SHOULD_HAVE_THE_SAME_NUMBER_OF_ROWS
        {
            get { return Resources.MATRIX_SHOULD_HAVE_THE_SAME_NUMBER_OF_ROWS; }
        }
        public String MATRIX_HAS_MORE_COLUMN_TNAN_ROWS
        {
            get { return Resources.MATRIX_HAS_MORE_COLUMN_TNAN_ROWS; }
        }
        public String MATRIX_ROW_DIMENSIONS_MUST_AGREE
        {
            get { return Resources.MATRIX_ROW_DIMENSIONS_MUST_AGREE; }
        }
        public String MATRIX_IS_RANK_DEFICIENT
        {
            get { return Resources.MATRIX_IS_RANK_DEFICIENT; }
        }
        public String NON_SQUARE_MATRIX
        {
            get { return Resources.NON_SQUARE_MATRIX; }
        }
        public String NON_SQUARE_OPERATOR
        {
            get { return Resources.NON_SQUARE_OPERATOR; }
        }
        public String NON_SYMMETRIC_MATRIX
        {
            get { return Resources.NON_SYMMETRIC_MATRIX; }
        }
        public String SINGULAR_MATRIX
        {
            get { return Resources.SINGULAR_MATRIX; }
        }
        public String SINGULAR_OPERATOR
        {
            get { return Resources.SINGULAR_OPERATOR; }
        }
        public String SINGULAR_VECTORS_WERE_NOT_COMPUTED
        {
            get { return Resources.SINGULAR_VECTORS_WERE_NOT_COMPUTED; }
        }
        public String UNSUPPORTED_OPERATION
        {
            get { return Resources.UNSUPPORTED_OPERATION; }
        }

    }
}
